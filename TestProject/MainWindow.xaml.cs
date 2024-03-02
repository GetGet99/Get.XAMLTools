using EnumsNET;
using Get.XAMLTools.Classes.Settings;
using Get.XAMLTools.Classes.Settings.Boolean;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TestProject;

public sealed partial class MainWindow : Microsoft.UI.Xaml.Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        SystemBackdrop = new MicaBackdrop();
    }
}

static class Settings
{
    public static OnOffSetting OnOffSettingTest = new("OnOffSetting Test")
    {
        Description = "Test Description",
        DefaultValue = true
    };
    public static CheckboxSetting CheckBoxSettingTest = new("CheckBoxSetting Test")
    {
        Description = "Test Description",
        DefaultValue = true
    };
    public static SelectSetting<TestEnum> SelectSettingTest = new("SelectSetting Test", Enums.GetValues<TestEnum>().ToArray())
    {
        Description = "Test Description",
        DefaultValue = TestEnum.Defaullt
    };
    public static readonly IEnumerable<Setting> AllSettings = new Setting[]
    {
        OnOffSettingTest,
        CheckBoxSettingTest,
        SelectSettingTest
    };
}

enum TestEnum
{
    [Display(Name = "Default Value"), Description("This is the default value.")]
    Defaullt,
    [Display(Name = "Value #1"), Description("This is the first value.")]
    Value1,
    [Display(Name = "Value #2"), Description("This is the second value.")]
    Value2
}