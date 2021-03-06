''' <summary>
''' TR-064 Support – Hosts
''' Date:  2020-12-01
''' <see cref="https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/hostsSCPD.pdf"/>
''' </summary>
Friend Class HostsSCPD
    Implements IHostsSCPD

    Private Property TR064Start As Func(Of SCPDFiles, String, Dictionary(Of String, String), Dictionary(Of String, String)) Implements IHostsSCPD.TR064Start
    Private ReadOnly Property ServiceFile As SCPDFiles = SCPDFiles.hostsSCPD Implements IHostsSCPD.Servicefile
    Private Property XML As Serializer

    Public Sub New(Start As Func(Of SCPDFiles, String, Dictionary(Of String, String), Dictionary(Of String, String)), XMLSerializer As Serializer)

        TR064Start = Start

        XML = XMLSerializer
    End Sub

    Public Function GetHostNumberOfEntries(ByRef HostNumberOfEntries As Integer) As Boolean Implements IHostsSCPD.GetHostNumberOfEntries
        Return TR064Start(ServiceFile, "GetHostNumberOfEntries", Nothing).TryGetValueEx("NewHostNumberOfEntries", HostNumberOfEntries)
    End Function

    Public Function GetSpecificHostEntry(MACAddress As String, ByRef Host As HostEntry) As Boolean Implements IHostsSCPD.GetSpecificHostEntry
        If Host Is Nothing Then Host = New HostEntry

        With TR064Start(ServiceFile, "GetSpecificHostEntry", New Dictionary(Of String, String) From {{"NewMACAddress", MACAddress}})

            Host.MACAddress = MACAddress

            Return .TryGetValueEx("NewIPAddress", Host.IPAddress) And
                   .TryGetValueEx("NewAddressSource", Host.AddressSource) And
                   .TryGetValueEx("NewLeaseTimeRemaining", Host.LeaseTimeRemaining) And
                   .TryGetValueEx("NewInterfaceType", Host.InterfaceType) And
                   .TryGetValueEx("NewActive", Host.Active) And
                   .TryGetValueEx("NewHostName", Host.HostName)

        End With
    End Function

    Public Function GetGenericHostEntry(Index As Integer, ByRef Host As HostEntry) As Boolean Implements IHostsSCPD.GetGenericHostEntry
        If Host Is Nothing Then Host = New HostEntry

        With TR064Start(ServiceFile, "GetGenericHostEntry", New Dictionary(Of String, String) From {{"NewIndex", Index}})

            Host.Index = Index

            Return .TryGetValueEx("NewMACAddress", Host.MACAddress) And
                   .TryGetValueEx("NewIPAddress", Host.IPAddress) And
                   .TryGetValueEx("NewAddressSource", Host.AddressSource) And
                   .TryGetValueEx("NewLeaseTimeRemaining", Host.LeaseTimeRemaining) And
                   .TryGetValueEx("NewInterfaceType", Host.InterfaceType) And
                   .TryGetValueEx("NewActive", Host.Active) And
                   .TryGetValueEx("NewHostName", Host.HostName)

        End With
    End Function

    Public Function GetChangeCounter(ByRef ChangeCounter As Integer) As Boolean Implements IHostsSCPD.GetChangeCounter
        Return TR064Start(ServiceFile, "X_AVM-DE_GetChangeCounter", Nothing).TryGetValueEx("NewX_AVM-DE_ChangeCounter", ChangeCounter)
    End Function

    Public Function GetAutoWakeOnLANByMACAddress(MACAddress As String, ByRef AutoWOLEnabled As Boolean) As Boolean Implements IHostsSCPD.GetAutoWakeOnLANByMACAddress
        Return TR064Start(ServiceFile, "X_AVM-DE_GetAutoWakeOnLANByMACAddress",
                          New Dictionary(Of String, String) From {{"NewMACAddress", MACAddress}}).
                          TryGetValueEx("NewAutoWOLEnabled", AutoWOLEnabled)


    End Function

    Public Function SetAutoWakeOnLANByMACAddress(MACAddress As String, AutoWOLEnabled As Boolean) As Boolean Implements IHostsSCPD.SetAutoWakeOnLANByMACAddress
        Return Not TR064Start(ServiceFile, "X_AVM-DE_SetAutoWakeOnLANByMACAddress", New Dictionary(Of String, String) From {{"NewMACAddress", MACAddress},
                                                                                                                            {"NewAutoWOLEnabled", AutoWOLEnabled.ToBoolStr}}).ContainsKey("Error")
    End Function

    Public Function SetHostNameByMACAddress(MACAddress As String, HostName As String) As Boolean Implements IHostsSCPD.SetHostNameByMACAddress
        Return Not TR064Start(ServiceFile, "X_AVM-DE_SetAutoWakeOnLANByMACAddress", New Dictionary(Of String, String) From {{"NewMACAddress", MACAddress},
                                                                                                                            {"NewHostName", HostName}}).ContainsKey("Error")
    End Function

    Public Function WakeOnLANByMACAddress(MACAddress As String) As Boolean Implements IHostsSCPD.WakeOnLANByMACAddress
        Return Not TR064Start(ServiceFile, "X_AVM-DE_WakeOnLANByMACAddress", New Dictionary(Of String, String) From {{"NewMACAddress", MACAddress}}).ContainsKey("Error")
    End Function

    Public Function GetSpecificHostEntryByIp(IPAddress As String, ByRef Host As HostEntry) As Boolean Implements IHostsSCPD.GetSpecificHostEntryByIp
        If Host Is Nothing Then Host = New HostEntry

        With TR064Start(ServiceFile, "X_AVM-DE_GetSpecificHostEntryByIp",
                        New Dictionary(Of String, String) From {{"NewIPAddress", IPAddress}})

            Host.IPAddress = IPAddress

            Return .TryGetValueEx("NewMACAddress", Host.MACAddress) And
                   .TryGetValueEx("NewActive", Host.Active) And
                   .TryGetValueEx("NewHostName", Host.HostName) And
                   .TryGetValueEx("NewInterfaceType", Host.InterfaceType) And
                   .TryGetValueEx("NewX_AVM-DE_Port", Host.Port) And
                   .TryGetValueEx("NewX_AVM-DE_Speed", Host.Speed) And
                   .TryGetValueEx("NewX_AVM-DE_UpdateAvailable", Host.UpdateAvailable) And
                   .TryGetValueEx("NewX_AVM-DE_UpdateSuccessful", Host.UpdateSuccessful) And
                   .TryGetValueEx("NewX_AVM-DE_InfoURL", Host.InfoURL) And
                   .TryGetValueEx("NewX_AVM-DE_Model", Host.Model) And
                   .TryGetValueEx("NewX_AVM-DE_URL", Host.URL)

        End With
    End Function

    Public Function HostsCheckUpdate() As Boolean Implements IHostsSCPD.HostsCheckUpdate
        Return Not TR064Start(ServiceFile, "X_AVM-DE_HostsCheckUpdate", Nothing).ContainsKey("Error")
    End Function

    Public Function HostDoUpdate(MACAddress As String) As Boolean Implements IHostsSCPD.HostDoUpdate
        Return Not TR064Start(ServiceFile, "X_AVM-DE_HostDoUpdate", New Dictionary(Of String, String) From {{"NewMACAddress", MACAddress}}).ContainsKey("Error")
    End Function

    Public Function GetHostListPath(ByRef HostListPath As String) As Boolean Implements IHostsSCPD.GetHostListPath
        Return TR064Start(ServiceFile, "X_AVM-DE_GetHostListPath", Nothing).TryGetValueEx("NewX_AVM-DE_HostListPath", HostListPath)
    End Function

    Public Function GetHostList(ByRef Hosts As HostList) As Boolean Implements IHostsSCPD.GetHostList
        Dim LuaPath As String = String.Empty
        Return GetHostListPath(LuaPath) AndAlso XML.Deserialize($"http://{FritzBoxTR64.FBoxIPAdresse}:{DfltTR064Port}{LuaPath}", True, Hosts)
    End Function

    Public Function GetMeshListPath(ByRef MeshListPath As String) As Boolean Implements IHostsSCPD.GetMeshListPath
        Return TR064Start(ServiceFile, "X_AVM-DE_GetMeshListPath", Nothing).TryGetValueEx("NewX_AVM-DE_MeshListPath", MeshListPath)
    End Function
End Class
