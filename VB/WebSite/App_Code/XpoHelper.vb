Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata

''' <summary>
''' Summary description for XpoHelper
''' </summary>
Public NotInheritable Class XpoHelper

    Private Sub New()
    End Sub

    Shared Sub New()
        CreateDefaultObjects()
    End Sub

    Public Shared Function GetNewSession() As Session
        Return New Session(DataLayer)
    End Function

    Public Shared Function GetNewUnitOfWork() As UnitOfWork
        Return New UnitOfWork(DataLayer)
    End Function

    Private ReadOnly Shared lockObject As New Object()

    Private Shared fDataLayer As IDataLayer
    Private Shared ReadOnly Property DataLayer() As IDataLayer
        Get
            If fDataLayer Is Nothing Then
                SyncLock lockObject
                    fDataLayer = GetDataLayer()
                End SyncLock
            End If
            Return fDataLayer
        End Get
    End Property

    Private Shared Function GetDataLayer() As IDataLayer
        XpoDefault.Session = Nothing

        Dim ds As New InMemoryDataStore()
        'string connectionString = AccessConnectionProvider.GetConnectionString(@"...\XPO_Membership\App_Data\my.mdb");
        'IDataStore ds = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.DatabaseAndSchema);

        Dim dict As XPDictionary = New ReflectionDictionary()
        dict.GetDataStoreSchema(GetType(XpoUser).Assembly)

        Return New ThreadSafeDataLayer(dict, ds)
    End Function

    Private Shared Sub CreateDefaultObjects()
        Dim status As MembershipCreateStatus = Nothing
        Membership.CreateUser("test", "test", "just@ask.me", "The answer is ""test""", "test", True, status)
        Membership.CreateUser("admin", "admin", "admin@ask.me", "The answer is ""admin""", "admin", True, status)

        'for (Int32 i = 0; i < 300; i++) {
        '    Membership.CreateUser(String.Format("test{0}", i), "test", String.Format("just{0}@ask.me", i), "The answer is \"test\"", "test", true, out status);
        '}
    End Sub
End Class