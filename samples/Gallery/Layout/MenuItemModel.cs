﻿using MauiIcons.Material.Rounded;

namespace Gallery.Layout;
public class MenuItemModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public MaterialRoundedIcons Icon { get; set; }
}