﻿namespace Core.ViewModels.ArticleViewModels;

public class ArticleViewModelBase
{
    public string? Title { get; set; }
    public string? Body { get; set; }
    public bool Published { get; set; }
}