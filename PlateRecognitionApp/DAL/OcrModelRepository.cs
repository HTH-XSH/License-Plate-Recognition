using System;
using System.Collections.Generic;
using System.IO;

namespace PlateRecognitionApp.DAL
{
    /// <summary>
    /// DAL 数据访问层：只负责定位 OCR 模型文件并确认文件存在，不负责读取 HALCON 分类器或识别字符。
    /// BLL 在开始 OCR 前调用本类取得路径；默认查找位置是程序运行目录，也就是生成的 exe 所在目录。
    /// </summary>
    internal sealed class OcrModelRepository
    {
        // 文件名集中在 DAL，避免窗体和识别流程各自拼写或维护不同路径。
        private const string ChineseModelFileName = "Gui_ocr.omc";
        private const string AlphanumericModelFileName = "Industrial_0-9A-Z_NoRej.omc";

        // 保存本次查找所依据的目录路径；它是普通 C# 字符串，不需要 Dispose。
        private readonly string _applicationDirectory;

        /// <summary>创建默认模型仓库，使用当前应用程序的基目录查找模型文件。</summary>
        public OcrModelRepository()
            : this(AppDomain.CurrentDomain.BaseDirectory)
        {
        }

        /// <summary>为本程序集中的调用方指定查找目录；生产流程通常使用无参构造方法。</summary>
        /// <param name="applicationDirectory">用于组合模型文件名的目录路径，不能为 null、空字符串或空白。</param>
        internal OcrModelRepository(string applicationDirectory)
        {
            if (string.IsNullOrWhiteSpace(applicationDirectory)) throw new ArgumentException("运行目录不能为空。", "applicationDirectory");
            _applicationDirectory = applicationDirectory;
        }

        /// <summary>
        /// 组合中文与字母数字模型的完整路径，并检查两个文件都存在。
        /// 本方法只检查文件系统，不会读取模型内容；实际加载由 BLL 调用 HALCON 完成。
        /// </summary>
        /// <param name="chineseModelPath">返回中文 OCR 模型的完整路径。</param>
        /// <param name="alphanumericModelPath">返回数字/英文字母 OCR 模型的完整路径。</param>
        /// <exception cref="FileNotFoundException">任一模型文件不存在时抛出，异常信息列出缺失文件及预期位置。</exception>
        public void GetModelPaths(out string chineseModelPath, out string alphanumericModelPath)
        {
            // 路径都相对于同一运行目录，避免依赖开发者电脑上的绝对路径。
            chineseModelPath = Path.Combine(_applicationDirectory, ChineseModelFileName);
            alphanumericModelPath = Path.Combine(_applicationDirectory, AlphanumericModelFileName);

            // 一次收集所有缺失项，让用户可以在同一次提示中补齐两个模型。
            List<string> missingFiles = new List<string>();
            if (!File.Exists(chineseModelPath)) missingFiles.Add(chineseModelPath);
            if (!File.Exists(alphanumericModelPath)) missingFiles.Add(alphanumericModelPath);

            if (missingFiles.Count > 0)
            {
                // 在进入 HALCON 读取算子前报告文件系统问题，区分“文件缺失”和“模型内容无法读取”。
                throw new FileNotFoundException("缺少 OCR 模型文件，请将以下文件放到程序运行目录（exe 所在文件夹）：\n" + string.Join("\n", missingFiles.ToArray()));
            }
        }
    }
}
