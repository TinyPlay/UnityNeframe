using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Queue Item View
/// </summary>
internal class QueueItem : MonoBehaviour
{
    // Context
    public struct Context
    {
        public string url;
        public string type;
    }
    private Context _ctx;

    // Private Params
    [SerializeField] private Text UrlLabel;
    [SerializeField] private Text TypeLabel;

    /// <summary>
    /// Set View Context
    /// </summary>
    /// <param name="ctx"></param>
    public void SetCtx(Context ctx)
    {
        _ctx = ctx;
        UpdateView();
    }

    /// <summary>
    /// Update View
    /// </summary>
    public void UpdateView()
    {
        UrlLabel.text = _ctx.url;
        TypeLabel.text = _ctx.type;
    }
}