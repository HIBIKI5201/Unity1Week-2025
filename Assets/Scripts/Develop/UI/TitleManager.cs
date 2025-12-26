using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UI Toolkit の VisualElement（Image コンテナ）を
/// 時間経過でフェードアニメーションさせるクラス
/// </summary>
public sealed class TitleManager : MonoBehaviour
{
    private VisualElement _imageContainer;
    private float _time;

    private void Start()
    {
        // UIDocument のルート要素を取得する
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Image をまとめた親 VisualElement を取得する
        _imageContainer = root.Q<VisualElement>("image-container");

        // UI Toolkit のスケジューラで定期実行する（約60fps）
        _imageContainer.schedule.Execute(AnimateOpacity).Every(8);
    }

    /// <summary>
    /// 親 VisualElement の opacity を往復アニメーションさせる
    /// </summary>
    private void AnimateOpacity()
    {
        _time += Time.deltaTime;

        // 0〜1 を往復する値を生成する
        float alpha = Mathf.PingPong(_time, 1f);

        // 親要素の透明度を変更する（子の Image 全体に影響する）
        _imageContainer.style.opacity = alpha;
    }
}