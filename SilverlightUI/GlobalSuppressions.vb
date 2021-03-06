'
'
' This file is used by Code Analysis to maintain SuppressMessage 
' attributes that are applied to this project.
' Project-level suppressions either have no target or are given 
' a specific target and scoped to a namespace, type, member, etc.
'
' To add a suppression to this file, right-click the message in the 
' Error List, point to "Suppress Message(s)", and click 
' "In Project Suppression File".
' You do not need to add suppressions to this file manually.

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope:="member", Target:="SilverlightUI.Web.User.#Global_System_Security_Principal_IIdentity_Name")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope:="type", Target:="SilverlightUI.Web.UserRegistrationContext+IUserRegistrationServiceContract")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Datas", Scope:="member", Target:="SilverlightUI.Web.UserRegistrationContext.#RegistrationDatas")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="SilverlightUI.Web.UserRegistrationContext.#GetUsersQuery()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope:="member", Target:="SilverlightUI.Web.UserRegistrationContext.#CreateUser(SilverlightUI.Web.RegistrationData,System.String,System.Action`1<System.ServiceModel.DomainServices.Client.InvokeOperation`1<SilverlightUI.Web.CreateUserStatus>>,System.Object)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope:="member", Target:="SilverlightUI.Web.User.#Global_System_Security_Principal_IIdentity_AuthenticationType")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="SilverlightUI.Web.RegistrationData.#ToLoginParameters()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.Web.RegistrationData.#PasswordConfirmationAccessor")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.Web.RegistrationData.#PasswordAccessor")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="SilverlightUI.Web.AuthenticationContext+IAuthenticationServiceContract.#EndLogout(System.IAsyncResult)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="SilverlightUI.Web.AuthenticationContext+IAuthenticationServiceContract.#EndLogin(System.IAsyncResult)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="SilverlightUI.Web.AuthenticationContext+IAuthenticationServiceContract.#BeginLogout(System.AsyncCallback,System.Object)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="SilverlightUI.Web.AuthenticationContext+IAuthenticationServiceContract.#BeginLogin(System.String,System.String,System.Boolean,System.String,System.AsyncCallback,System.Object)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope:="type", Target:="SilverlightUI.Web.AuthenticationContext+IAuthenticationServiceContract")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="SilverlightUI.Web.AuthenticationContext.#LogoutQuery()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="SilverlightUI.Web.AuthenticationContext.#LoginQuery(System.String,System.String,System.Boolean,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="SilverlightUI.Web.AuthenticationContext.#GetUserQuery()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.RegistrationForm.#RegisterForm_AutoGeneratingField(System.Object,System.Windows.Controls.DataFormAutoGeneratingFieldEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.RegistrationForm.#RegisterButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.RegistrationForm.#CreateComboBoxWithSecurityQuestions()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.RegistrationForm.#CancelButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.RegistrationForm.#BackToLogin_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginStatus.#LogoutButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginStatus.#LoginButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="SilverlightUI.LoginUI.LoginStatus.#.ctor()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="SilverlightUI.LoginUI.LoginStatus")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="SilverlightUI.LoginUI.LoginRegistrationWindow.#NavigateToLogin()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="SilverlightUI.LoginUI.LoginRegistrationWindow.#.ctor()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="SilverlightUI.LoginUI.LoginRegistrationWindow")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="SilverlightUI.LoginUI.LoginInfo.#ToLoginParameters()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginInfo.#PasswordAccessor")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginInfo.#CurrentLoginOperation")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="LogIn", Scope:="member", Target:="SilverlightUI.LoginUI.LoginInfo.#CanLogIn")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="SilverlightUI.LoginUI.LoginInfo")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginForm.#RegisterNow_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginForm.#LoginForm_KeyDown(System.Object,System.Windows.Input.KeyEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginForm.#LoginForm_AutoGeneratingField(System.Object,System.Windows.Controls.DataFormAutoGeneratingFieldEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginForm.#LoginButton_Click(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.LoginUI.LoginForm.#CancelButton_Click(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="SilverlightUI.LoginUI.LoginForm")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope:="member", Target:="SilverlightUI.TargetNullValueConverter.#Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId:="string", Scope:="member", Target:="SilverlightUI.StringFormatValueConverter.#.ctor(System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="SilverlightUI.ResourceWrapper.#SecurityQuestions")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="SilverlightUI.ResourceWrapper.#ApplicationStrings")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.MainPage.#ContentFrame_NavigationFailed(System.Object,System.Windows.Navigation.NavigationFailedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.MainPage.#ContentFrame_Navigated(System.Object,System.Windows.Navigation.NavigationEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="SilverlightUI.MainPage.#.ctor()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.ErrorWindow.#OKButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="SilverlightUI.ErrorWindow.#.ctor(System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope:="member", Target:="SilverlightUI.DataFieldExtensions.#ReplaceTextBox(System.Windows.Controls.DataField,System.Windows.FrameworkElement,System.Windows.DependencyProperty,System.Action`1<System.Windows.Data.Binding>)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope:="member", Target:="SilverlightUI.DataBindingExtensions.#CreateOneWayBinding(System.ComponentModel.INotifyPropertyChanged,System.String,System.Windows.Data.IValueConverter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.App.#Application_UnhandledException(System.Object,System.Windows.ApplicationUnhandledExceptionEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="SilverlightUI.App.#Application_Startup(System.Object,System.Windows.StartupEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="namespace", Target:="SilverlightUI.LoginUI")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="SilverlightUI.Controls")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope:="member", Target:="SilverlightUI.Web.User.#Global_System_Security_Principal_IPrincipal_Identity")> 

