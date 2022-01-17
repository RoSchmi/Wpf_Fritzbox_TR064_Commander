﻿''' <summary>
''' TR-064 Support – WANCommonInterfaceConfig
''' Date: 2018-09-05
''' <see href="link">https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/wancommonifconfigSCPD.pdf</see>
''' </summary>
Friend Class WANCommonInterfaceConfigSCPD
    Implements IWANCommonInterfaceConfigSCPD
    Private Property TR064Start As Func(Of SCPDFiles, String, Dictionary(Of String, String), Dictionary(Of String, String)) Implements IWANCommonInterfaceConfigSCPD.TR064Start
    Private ReadOnly Property ServiceFile As SCPDFiles = SCPDFiles.wancommonifconfigSCPD Implements IWANCommonInterfaceConfigSCPD.Servicefile

    Public Sub New(Start As Func(Of SCPDFiles, String, Dictionary(Of String, String), Dictionary(Of String, String)))

        TR064Start = Start

    End Sub

    Public Function GetCommonLinkProperties(ByRef WANAccessType As AccessTypeEnum,
                                            ByRef Layer1UpstreamMaxBitRate As Integer,
                                            ByRef Layer1DownstreamMaxBitRate As Integer,
                                            ByRef PhysicalLinkStatus As PhysicalLinkStatusEnum) As Boolean Implements IWANCommonInterfaceConfigSCPD.GetCommonLinkProperties

        With TR064Start(ServiceFile, "GetCommonLinkProperties", Nothing)

            Return .TryGetValue("NewWANAccessType", WANAccessType) And
                   .TryGetValue("NewLayer1UpstreamMaxBitRate", Layer1UpstreamMaxBitRate) And
                   .TryGetValue("NewLayer1DownstreamMaxBitRate", Layer1DownstreamMaxBitRate) And
                   .TryGetValue("NewPhysicalLinkStatus", PhysicalLinkStatus)

        End With

    End Function

    Public Function GetTotalBytesSent(ByRef TotalBytesSent As Integer) As Boolean Implements IWANCommonInterfaceConfigSCPD.GetTotalPacketsSent
        Return TR064Start(ServiceFile, "GetTotalBytesSent", Nothing).TryGetValue("NewTotalBytesSent", TotalBytesSent)
    End Function

    Public Function GetTotalBytesReceived(ByRef TotalBytesReceived As Integer) As Boolean Implements IWANCommonInterfaceConfigSCPD.GetTotalBytesReceived
        Return TR064Start(ServiceFile, "GetTotalBytesReceived", Nothing).TryGetValue("NewTotalBytesReceived", TotalBytesReceived)
    End Function

    Public Function GetTotalPacketsSent(ByRef TotalPacketsSent As Integer) As Boolean Implements IWANCommonInterfaceConfigSCPD.GetTotalBytesSent
        Return TR064Start(ServiceFile, "GetTotalPacketsSent", Nothing).TryGetValue("NewTotalPacketsSent", TotalPacketsSent)
    End Function

    Public Function GetTotalPacketsReceived(ByRef TotalPacketsReceived As Integer) As Boolean Implements IWANCommonInterfaceConfigSCPD.GetTotalPacketsReceived
        Return TR064Start(ServiceFile, "GetTotalPacketsReceived", Nothing).TryGetValue("NewTotalPacketsReceived", TotalPacketsReceived)
    End Function

    Public Function SetWANAccessType(AccessType As AccessTypeEnum) As Boolean Implements IWANCommonInterfaceConfigSCPD.SetWANAccessType
        Return Not TR064Start(ServiceFile, "X_AVM-DE_SetWANAccessType", New Dictionary(Of String, String) From {{"NewAccessType", AccessType}}).ContainsKey("Error")
    End Function

    Public Function GetOnlineMonitor(ByRef OnlineMonitor As OnlineMonitor, SyncGroupIndex As Integer) As Boolean Implements IWANCommonInterfaceConfigSCPD.GetOnlineMonitor
        If OnlineMonitor Is Nothing Then OnlineMonitor = New OnlineMonitor

        With TR064Start(ServiceFile, "X_AVM-DE_GetOnlineMonitor", New Dictionary(Of String, String) From {{"NewSyncGroupIndex", SyncGroupIndex}})

            OnlineMonitor.SyncGroupIndex = SyncGroupIndex

            Return .TryGetValue("NewTotalNumberSyncGroups", OnlineMonitor.TotalNumberSyncGroups) And
                   .TryGetValue("NewSyncGroupName", OnlineMonitor.SyncGroupName) And
                   .TryGetValue("NewSyncGroupMode", OnlineMonitor.SyncGroupMode) And
                   .TryGetValue("Newmax_ds", OnlineMonitor.Max_ds) And
                   .TryGetValue("Newmax_us", OnlineMonitor.Max_us) And
                   .TryGetValue("Newds_current_bps", OnlineMonitor.Ds_current_bps) And
                   .TryGetValue("Newmc_current_bps", OnlineMonitor.Mc_current_bps) And
                   .TryGetValue("Newus_current_bps", OnlineMonitor.Us_current_bps) And
                   .TryGetValue("Newprio_realtime_bps", OnlineMonitor.Prio_realtime_bps) And
                   .TryGetValue("Newprio_high_bps", OnlineMonitor.Prio_high_bps) And
                   .TryGetValue("Newprio_default_bps", OnlineMonitor.Prio_default_bps) And
                   .TryGetValue("Newprio_low_bps", OnlineMonitor.Prio_low_bps)
        End With
    End Function
End Class