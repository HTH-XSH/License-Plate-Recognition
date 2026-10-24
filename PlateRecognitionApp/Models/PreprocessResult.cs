using System;
using HalconDotNet;

namespace PlateRecognitionApp.Models
{
    /// <summary>
    /// Models 层的预处理结果：把 OCR 所需的排序字符区域、反色图像和字符数量作为一个整体传给 UI。
    /// 本对象拥有两个 HObject；窗体只在加载新图、重选 ROI、重做预处理或关闭时 Dispose 整个结果，
    /// 不应单独释放属性返回的 HObject，也不能在 Dispose 后继续读取这些属性。
    /// </summary>
    internal sealed class PreprocessResult : IDisposable
    {
        // 两个由业务层移交给本结果对象的 HALCON 资源；本类统一负责释放。
        private HObject _sortedCharacters;
        private HObject _ocrImage;

        // Dispose 设置此标记，保证重复释放安全，并阻止通过属性访问已释放的对象。
        private bool _disposed;

        /// <summary>
        /// 创建并接管一次预处理所需的 HALCON 对象。
        /// 两个参数必须非 null；构造成功后释放责任转到本结果对象。
        /// </summary>
        /// <param name="sortedCharacters">按读取顺序排列的字符区域，构造成功后由本对象拥有。</param>
        /// <param name="ocrImage">供 OCR 提取灰度值的反色图像，构造成功后由本对象拥有。</param>
        /// <param name="characterCount">字符区域数量的普通整数快照，不拥有 HALCON 资源。</param>
        /// <exception cref="ArgumentNullException">任一 HALCON 对象为空时抛出。</exception>
        public PreprocessResult(HObject sortedCharacters, HObject ocrImage, int characterCount)
        {
            if (sortedCharacters == null) throw new ArgumentNullException("sortedCharacters");
            if (ocrImage == null) throw new ArgumentNullException("ocrImage");

            _sortedCharacters = sortedCharacters;
            _ocrImage = ocrImage;
            CharacterCount = characterCount;
        }

        /// <summary>取得按预定字符顺序排列的区域集合；返回引用仍由本对象拥有，调用方不得 Dispose。</summary>
        /// <exception cref="ObjectDisposedException">本结果对象已经 Dispose 时抛出。</exception>
        public HObject SortedCharacters
        {
            get { ThrowIfDisposed(); return _sortedCharacters; }
        }

        /// <summary>取得 OCR 用来读取字符灰度的反色图像；返回引用仍由本对象拥有，调用方不得 Dispose。</summary>
        /// <exception cref="ObjectDisposedException">本结果对象已经 Dispose 时抛出。</exception>
        public HObject OcrImage
        {
            get { ThrowIfDisposed(); return _ocrImage; }
        }

        /// <summary>本次预处理得到的候选区域数量；仅为整数，不需要释放。</summary>
        public int CharacterCount { get; private set; }

        /// <summary>释放本对象拥有的字符区域和 OCR 图像；重复调用不会重复释放。</summary>
        public void Dispose()
        {
            // 已经清理过时直接返回；此状态也用于拒绝后续属性访问。
            if (_disposed) return;

            _disposed = true;
            // 先从字段取出并清空引用，随后统一释放；清空引用本身不等同于 HALCON Dispose。
            HObject sortedCharacters = _sortedCharacters;
            HObject ocrImage = _ocrImage;
            _sortedCharacters = null;
            _ocrImage = null;

            try
            {
                if (sortedCharacters != null) sortedCharacters.Dispose();
            }
            finally
            {
                // 即使释放字符区域时抛出异常，仍尝试释放 OCR 图像。
                if (ocrImage != null) ocrImage.Dispose();
            }
        }

        /// <summary>检查结果对象是否仍可使用。</summary>
        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException("PreprocessResult");
        }
    }
}
