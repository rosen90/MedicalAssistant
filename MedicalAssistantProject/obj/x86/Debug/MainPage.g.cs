﻿

#pragma checksum "C:\Users\karad\OneDrive\Documents\Visual Studio 2015\Projects\MedicalAssistantProject\MedicalAssistantProject\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "69C079410E83E0206A89AA46CEE531F9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalAssistantProject
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 15 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 16 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.RateApp_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 24 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Pivot)(target)).SelectionChanged += this.PivotLayout_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 348 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.Hospitals_ItemClick;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 375 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Navigate_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 313 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.SearchBoxMed_TextChanged;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 316 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.MedList_ItemClick;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 274 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.SearchBoxIll_TextChanged;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 277 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.IllnessList_ItemClick;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 234 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.SearchBox_TextChanged;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 237 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.DoctorList_ItemClick;
                 #line default
                 #line hidden
                break;
            case 12:
                #line 55 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.HeadClick_Click;
                 #line default
                 #line hidden
                break;
            case 13:
                #line 92 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.LegsClicked_Click;
                 #line default
                 #line hidden
                break;
            case 14:
                #line 102 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SearchBtn_Click;
                 #line default
                 #line hidden
                break;
            case 15:
                #line 126 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).DoubleTapped += this.listView_DoubleTapped;
                 #line default
                 #line hidden
                #line 126 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.SymptChoices_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 16:
                #line 167 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.comboBox_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 17:
                #line 187 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ArmsClicked_Click;
                 #line default
                 #line hidden
                break;
            case 18:
                #line 198 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ArmsClicked_Click;
                 #line default
                 #line hidden
                break;
            case 19:
                #line 208 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.TorsoClicked_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


