Public Class Form1
    Declare Function UnhookWindowsHookEx Lib "user32" Alias "UnhookWindowsHookEx" (ByVal hHook As IntPtr) As Boolean
    Delegate Function LowLevelKeyboardProcDelegate(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer
    Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Integer, ByVal lpfn As LowLevelKeyboardProcDelegate, ByVal hMod As IntPtr, ByVal dwThreadId As Integer) As IntPtr
    Declare Function CallNextHookEx Lib "user32" Alias "CallNextHookEx" (ByVal hHook As IntPtr, ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer
    Const WH_KEYBOARD_LL As Integer = 13
    Dim intLLKey As IntPtr
    Structure KBDLLHOOKSTRUCT
        Dim vkCode As Integer
        Dim scanCode As Integer
        Dim flags As Integer
        Dim time As Integer
        Dim dwExtraInfo As Integer
    End Structure
    Private Function LowLevelKeyboardProc(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer
        Dim blnEat As Boolean = False

        Select Case wParam
            Case 256, 257, 260, 261
                blnEat = ((lParam.vkCode = 9) AndAlso (lParam.flags = 32)) Or
                ((lParam.vkCode = 27) AndAlso (lParam.flags = 32)) Or
                ((lParam.vkCode = 27) AndAlso (lParam.flags = 0)) Or
                ((lParam.vkCode = 91) AndAlso (lParam.flags = 1)) Or
                ((lParam.vkCode = 92) AndAlso (lParam.flags = 1))
        End Select

        If blnEat = True Then
            Return 1
        Else
            Return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam)
        End If
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        intLLKey = SetWindowsHookEx(WH_KEYBOARD_LL, AddressOf LowLevelKeyboardProc, IntPtr.Zero, 0)
        UnhookWindowsHookEx(intLLKey)
        Interaction.Shell("explorer.exe")
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLockWorkstation", "0", Microsoft.Win32.RegistryValueKind.DWord)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableChangePassword", "0", Microsoft.Win32.RegistryValueKind.DWord)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "NoClose", "0", Microsoft.Win32.RegistryValueKind.DWord)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "NoLogoff", "0", Microsoft.Win32.RegistryValueKind.DWord)
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", "0", Microsoft.Win32.RegistryValueKind.DWord)
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", "1", Microsoft.Win32.RegistryValueKind.DWord)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableRegistryTools", "0", Microsoft.Win32.RegistryValueKind.DWord)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableTaskMgr", "0", Microsoft.Win32.RegistryValueKind.DWord)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\System", "DisableCMD", "0", Microsoft.Win32.RegistryValueKind.DWord)
        MsgBox("Votre PC a été débloqué.", MsgBoxStyle.Information, "Information")
    End Sub
End Class
