using AmongUs.Data;
using Assets.InnerNet;
using BepInEx;
using BepInEx.Unity.IL2CPP.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMPro;
using Twitch;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheOtherRoles.Modules;

public class ModUpdater : MonoBehaviour
{
    public const string RepositoryOwner = "fangkuaiclub";
    public const string RepositoryName = "TheOtherRoles-X";

    private long _lastDownloadBytes;
    private float _lastDownloadTime;

    private bool _busy;
    public List<GithubRelease> Releases;
    private bool showPopUp = true;

    public ModUpdater(IntPtr ptr) : base(ptr)
    {
    }

    public static ModUpdater Instance { get; private set; }

    public void Awake()
    {
        if (Instance) Destroy(Instance);
        Instance = this;
        foreach (var file in Directory.GetFiles(Paths.PluginPath, "*.old")) File.Delete(file);
    }

    private void Start()
    {
        if (_busy) return;
        this.StartCoroutine(CoCheckForUpdate());
        SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>)OnSceneLoaded);
    }


    [HideFromIl2Cpp]
    public void StartDownloadRelease(GithubRelease release)
    {
        if (_busy) return;
        this.StartCoroutine(CoDownloadRelease(release));
    }

    [HideFromIl2Cpp]
    private IEnumerator CoCheckForUpdate()
    {
        _busy = true;
        var www = new UnityWebRequest();
        www.SetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
        www.SetUrl($"https://api.github.com/repos/{RepositoryOwner}/{RepositoryName}/releases");
        www.downloadHandler = new DownloadHandlerBuffer();
        var operation = www.SendWebRequest();

        while (!operation.isDone) yield return new WaitForEndOfFrame();

        if (www.isNetworkError || www.isHttpError) yield break;

        Releases = JsonSerializer.Deserialize<List<GithubRelease>>(www.downloadHandler.text);
        www.downloadHandler.Dispose();
        www.Dispose();
        Releases.Sort(SortReleases);
        _busy = false;
    }

    [HideFromIl2Cpp]
    private IEnumerator CoDownloadRelease(GithubRelease release)
    {
        _busy = true;
        _lastDownloadBytes = 0;
        _lastDownloadTime = 0;

        var popup = Instantiate(TwitchManager.Instance.TwitchPopup);
        popup.TextAreaTMP.fontSize *= 0.7f;
        popup.TextAreaTMP.enableAutoSizing = false;
        popup.name = "Mod Updater Popup";
        popup.TextAreaTMP.transform.localPosition = new Vector3(0f, 0.28f, -0.5f);

        popup.Show();

        var button = popup.transform.GetChild(2).gameObject;
        button.SetActive(false);
        popup.TextAreaTMP.text = $"updatingText".Translate();

        var asset = release.Assets.Find(FilterPluginAsset);
        var www = new UnityWebRequest();
        www.SetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
        www.SetUrl(Helpers.isChinese() ? "https://ghproxy.fangkuai.fun/" + asset.DownloadUrl : asset.DownloadUrl);
        www.downloadHandler = new DownloadHandlerBuffer();
        var operation = www.SendWebRequest();

        _lastDownloadTime = Time.realtimeSinceStartup;

        while (!operation.isDone)
        {
            int stars = Mathf.CeilToInt(www.downloadProgress * 10);

            long currentBytes = (long)www.downloadedBytes;
            float currentTime = Time.realtimeSinceStartup;

            var downloadText = new String((char)0x25A0, stars) + new String((char)0x25A1, 10 - stars);
            if (_lastDownloadTime > 0 && _lastDownloadBytes > 0)
            {
                float timeDelta = currentTime - _lastDownloadTime;
                long bytesDelta = currentBytes - _lastDownloadBytes;

                if (timeDelta > 0)
                {
                    float speed = bytesDelta / timeDelta;
                    string speedText;
                    if (speed > 1048576)
                    {
                        speedText = $"{(speed / 1048576):0.00} MB/s";
                    }
                    else
                    {
                        speedText = $"{(speed / 1024):0.00} KB/s";
                    }
                    popup.TextAreaTMP.text = string.Format("downliadingText".Translate(), downloadText, speedText);
                }
            }
            else
            {
                var speedText = "0.00 KB/s";
                popup.TextAreaTMP.text = string.Format("downliadingText".Translate(), downloadText, speedText);
            }

            _lastDownloadBytes = currentBytes;
            _lastDownloadTime = currentTime;

            yield return new WaitForEndOfFrame();
        }

        if (www.isNetworkError || www.isHttpError)
        {
            popup.TextAreaTMP.text = "updateFailedText".Translate();
            yield break;
        }
        popup.TextAreaTMP.text = $"copyingFileText".Translate();

        var filePath = Path.Combine(Paths.PluginPath, asset.Name);

        if (File.Exists(filePath + ".old")) File.Delete(filePath + "old");
        if (File.Exists(filePath)) File.Move(filePath, filePath + ".old");

        var persistTask = File.WriteAllBytesAsync(filePath, Helpers.GetUnstrippedData(www.downloadHandler));
        var hasError = false;
        while (!persistTask.IsCompleted)
        {
            if (persistTask.Exception != null)
            {
                hasError = true;
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        www.downloadHandler.Dispose();
        www.Dispose();

        if (!hasError)
        {
            popup.TextAreaTMP.text = $"updateEndText".Translate();
        }
        button.SetActive(true);
        _busy = false;
    }

    [HideFromIl2Cpp]
    private static bool FilterLatestRelease(GithubRelease release)
    {
        return release.IsNewer(TheOtherRolesPlugin.Version) && release.Assets.Any(FilterPluginAsset);
    }

    [HideFromIl2Cpp]
    private static bool FilterPluginAsset(GithubAsset asset)
    {
        return asset.Name == "TheOtherRoles.dll";
    }

    [HideFromIl2Cpp]
    private static int SortReleases(GithubRelease a, GithubRelease b)
    {
        if (a.IsNewer(b.Version)) return -1;
        if (b.IsNewer(a.Version)) return 1;
        return 0;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_busy || scene.name != "MainMenu") return;
        var latestRelease = Releases.FirstOrDefault();
        if (latestRelease == null || latestRelease.Version <= TheOtherRolesPlugin.Version)
            return;

#if PC
        var template = GameObject.Find("ExitGameButton");
        if (!template) return;

        var button = Instantiate(template, null);
        var buttonTransform = button.transform;
        //buttonTransform.localPosition = new Vector3(-2f, -2f);
        button.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.458f, 0.124f);

        var passiveButton = button.GetComponent<PassiveButton>();
        passiveButton.OnClick = new Button.ButtonClickedEvent();
        passiveButton.OnClick.AddListener((Action)(() =>
        {
            StartDownloadRelease(latestRelease);
            button.SetActive(false);
        }));

        var text = button.transform.GetComponentInChildren<TMP_Text>();
        var t = "updateButtonText".Translate();
        StartCoroutine(Effects.Lerp(0.1f, (Action<float>)(p => text.SetText(t))));
        passiveButton.OnMouseOut.AddListener((Action)(() => text.color = Color.red));
        passiveButton.OnMouseOver.AddListener((Action)(() => text.color = Color.white));
#endif
        string parsedDescription = ParseLocalizedDescription(latestRelease.Description);
        var announcement = string.Format("announcementText".Translate(), latestRelease.Tag, parsedDescription);
        var mgr = FindObjectOfType<MainMenuManager>(true);
        if (showPopUp)
            mgr.StartCoroutine(CoShowAnnouncement(announcement, shortTitle: "TOR Update",
                date: latestRelease.PublishedAt));
        showPopUp = false;
    }

    [HideFromIl2Cpp]
    private string ParseLocalizedDescription(string rawDescription)
    {
        try
        {
            var lang = DataManager.Settings.Language.CurrentLanguage;
            string targetSection = lang == SupportedLangs.SChinese ? "### CN" : "### EN";
            string nextSection = lang == SupportedLangs.SChinese ? "### EN" : "### CN";

            string normalized = rawDescription.Replace("\r\n", "\n");
            int startIndex = normalized.IndexOf(targetSection);

            if (startIndex == -1) return rawDescription;

            startIndex += targetSection.Length;
            int endIndex = normalized.IndexOf("###", startIndex);

            string result = endIndex == -1 ?
                normalized.Substring(startIndex) :
                normalized.Substring(startIndex, endIndex - startIndex);

            return result.Trim('\n', '\r', ' ')
                         .Replace("\n#", "\n##");
        }
        catch
        {
            return rawDescription;
        }
    }

    [HideFromIl2Cpp]
    public IEnumerator CoShowAnnouncement(string announcement, bool show = true, string shortTitle = "TOR Update",
        string title = "", string date = "")
    {
        var mgr = FindObjectOfType<MainMenuManager>(true);
        var popUpTemplate = FindObjectOfType<AnnouncementPopUp>(true);
        if (popUpTemplate == null)
        {
            TheOtherRolesPlugin.Logger.LogError("couldnt show credits, popUp is null");
            yield return null;
        }

        var popUp = Instantiate(popUpTemplate);

        popUp.gameObject.SetActive(true);

        Announcement creditsAnnouncement = new()
        {
            Id = "torAnnouncement",
            Language = 0,
            Number = 6969,
            Title = title == "" ? "TheOtherRoles Announcement" : title,
            ShortTitle = shortTitle,
            SubTitle = "",
            PinState = false,
            Date = date == "" ? DateTime.Now.Date.ToString() : date,
            Text = announcement
        };
        mgr.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p =>
        {
            if (p == 1)
            {
                var backup = DataManager.Player.Announcements.allAnnouncements;
                DataManager.Player.Announcements.allAnnouncements =
                    new Il2CppSystem.Collections.Generic.List<Announcement>();
                popUp.Init(false);
                DataManager.Player.Announcements.SetAnnouncements(new[] { creditsAnnouncement });
                popUp.CreateAnnouncementList();
                popUp.UpdateAnnouncementText(creditsAnnouncement.Number);
                popUp.visibleAnnouncements[0].PassiveButton.OnClick.RemoveAllListeners();
                DataManager.Player.Announcements.allAnnouncements = backup;
            }
        })));
    }
}

public class GithubRelease
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("tag_name")] public string Tag { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("draft")] public bool Draft { get; set; }

    [JsonPropertyName("prerelease")] public bool Prerelease { get; set; }

    [JsonPropertyName("created_at")] public string CreatedAt { get; set; }

    [JsonPropertyName("published_at")] public string PublishedAt { get; set; }

    [JsonPropertyName("body")] public string Description { get; set; }

    [JsonPropertyName("assets")] public List<GithubAsset> Assets { get; set; }

    public Version Version => Version.Parse(Tag.Replace("v", string.Empty));

    public bool IsNewer(Version version)
    {
        return Version > version;
    }
}

public class GithubAsset
{
    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string DownloadUrl { get; set; }
}