namespace PlateRecognitionApp.Models
{
    /// <summary>
    /// Models 层的 OCR 输出：把识别字符串与参与识别的字符数一起交给 UI 显示。
    /// 属性只包含普通 C# 值，不持有 HObject、HTuple 或分类器句柄，因此无需实现 Dispose。
    /// </summary>
    internal sealed class PlateRecognitionResult
    {
        /// <summary>创建一次识别的托管结果。</summary>
        /// <param name="plateNumber">按预处理字符顺序拼接的识别字符串。</param>
        /// <param name="characterCount">本次处理的候选字符数量。</param>
        public PlateRecognitionResult(string plateNumber, int characterCount)
        {
            PlateNumber = plateNumber;
            CharacterCount = characterCount;
        }

        /// <summary>OCR 逐字符拼接得到的车牌文本。</summary>
        public string PlateNumber { get; private set; }

        /// <summary>本次识别处理的字符区域数量。</summary>
        public int CharacterCount { get; private set; }
    }
}
