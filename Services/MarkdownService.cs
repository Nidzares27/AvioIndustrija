﻿using Markdig;

namespace AvioIndustrija.Services
{
    public class MarkdownService
    {
        public string ConvertToHtml(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(markdown, pipeline);
        }
    }
}


