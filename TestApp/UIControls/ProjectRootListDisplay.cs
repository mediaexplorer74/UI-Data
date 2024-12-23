using Get.ProjectPlanner.DiskWrapper;
using Get.UI.Data;
using Windows.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls;
using ProgressRing = Microsoft.UI.Xaml.Controls.ProgressRing;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using static Get.UI.Data.QuickCreate;
using NavigationViewBackButtonVisible = Microsoft.UI.Xaml.Controls.NavigationViewBackButtonVisible;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
namespace Get.ProjectPlanner.UIControls;

partial class ProjectRootListDisplay(FolderAsyncCollection<ProjectRoot> col) : TemplateControl<NavigationView>
{
    protected override async void Initialize(NavigationView nav)
    {
        var count = await col.GetCountAsync();
        var ele = await col.GetAsync(0);
        static AsyncTextBox CreateTB(IAsyncProperty<string> title)
        {
            return new AsyncTextBox(title, placeholder: "Project Title", asTextBlock: true);
        }
        nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(60);
        nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(60); nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(9); nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(9); nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(9); nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(9); nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(9); nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(9); nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(9); nav.MenuItems.Add(new NavigationViewItem { Content = "Hello World!" });
        await Task.Delay(9);
        var newBtn = new NavigationViewItem() { SelectsOnInvoked = false, Icon = new SymbolIcon(Symbol.Add), Content = "New Project" };
        nav.FooterMenuItems.Add(
            newBtn
        );
    }
}
static class Extension
{
    public static T ForceWait<T>(this Task<T> task)
    {
        task.Wait();
        return task.Result;
    }
}