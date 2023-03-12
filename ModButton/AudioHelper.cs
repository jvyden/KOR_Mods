using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModButton;

public static class AudioHelper
{
    private static readonly Dictionary<string, AudioClip> _clipCache = new();

    private static IEnumerator LoadFileFromPath(string path, Action<AudioClip> callback)
    {
        using UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            ModButtonPlugin._Logger.LogError("Request did not succeed, result was " + req.result);
            ModButtonPlugin._Logger.LogError(req.error);
            yield break;
        }

        AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
        callback(clip);
    }

    public static IEnumerator LoadAndPlayFile(MonoBehaviour instance, string path)
    {
        if (!_clipCache.ContainsKey(path))
        {
            instance.StartCoroutine(LoadFileFromPath(path, (clip) =>
            {
                _clipCache.Add(path, clip);
                instance.StartCoroutine(LoadAndPlayFile(instance, path));
            }));
            yield break;
        }

        AudioClip clip = _clipCache[path];

        AudioSource source = instance.GetComponent<AudioSource>();
        if (source == null) source = instance.gameObject.AddComponent<AudioSource>();
            
        source.PlayOneShot(clip);
    }
}