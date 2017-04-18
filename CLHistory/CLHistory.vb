Imports pbs.Helper
Imports System.Data
Imports System.Data.SqlClient
Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Script
Imports pbs.BO.BusinessRules


Namespace EXT.VUS

    <Serializable()> _
    <DB(TableName:="pbs_EXT_VUS_CLHISTORY_{XXX}")>
    Public Class CLHistory
        Inherits Csla.BusinessBase(Of CLHistory)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink



#Region "Property Changed"
        Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
        End Sub

        Private Sub BO_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
            'Select Case e.PropertyName

            '    Case "OrderType"
            '        If Not Me.GetOrderTypeInfo.ManualRef Then
            '            Me._orderNo = POH.AutoReference
            '        End If

            '    Case "OrderDate"
            '        If String.IsNullOrEmpty(Me.OrderPrd) Then Me._orderPrd.Text = Me._orderDate.Date.ToSunPeriod

            '    Case "SuppCode"
            '        For Each line In Lines
            '            line._suppCode = Me.SuppCode
            '        Next

            '    Case "ConvCode"
            '        If String.IsNullOrEmpty(Me.ConvCode) Then
            '            _convRate.Float = 0
            '        Else
            '            Dim conv = pbs.BO.LA.CVInfoList.GetConverter(Me.ConvCode, _orderPrd, String.Empty)
            '            If conv IsNot Nothing Then
            '                _convRate.Float = conv.DefaultRate
            '            End If
            '        End If

            '    Case Else

            'End Select

            pbs.BO.Rules.CalculationRules.Calculator(sender, e)
        End Sub
#End Region

#Region " Business Properties and Methods "
        Private _DTB As String = String.Empty


        Private _classHistoryId As Integer
        <System.ComponentModel.DataObjectField(True, False)> _
        <Rule(Required:=True)>
        Public ReadOnly Property ClassHistoryId() As String
            Get
                Return _classHistoryId
            End Get
        End Property

        Private _changedDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar)>
        Public Property ChangedDate() As String
            Get
                Return _changedDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ChangedDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _changedDate.Equals(value) Then
                    _changedDate.Text = value
                    PropertyHasChanged("ChangedDate")
                End If
            End Set
        End Property

        Private _changedTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public Property ChangedTime() As String
            Get
                Return String.Format("{0:HH-mm-ss}", _changedTime)
            End Get
            Set(value As String)
                CanWriteProperty("ChangedTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _changedTime.Equals(value) Then
                    _changedTime.Text = value
                    PropertyHasChanged("ChangedTime")
                End If
            End Set
        End Property

        Private _classId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public Property ClassId() As String
            Get
                Return _classId.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ClassId", True)
                If value Is Nothing Then value = String.Empty
                If Not _classId.Equals(value) Then
                    _classId.Text = value
                    PropertyHasChanged("ClassId")
                End If
            End Set
        End Property

        Private _classCode As String = String.Empty
        Public Property ClassCode() As String
            Get
                Return _classCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ClassCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _classCode.Equals(value) Then
                    _classCode = value
                    PropertyHasChanged("ClassCode")
                End If
            End Set
        End Property

        Private _openDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar)>
        Public Property OpenDate() As String
            Get
                Return _openDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("OpenDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _openDate.Equals(value) Then
                    _openDate.Text = value
                    PropertyHasChanged("OpenDate")
                End If
            End Set
        End Property

        Private _closeDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar)>
        Public Property CloseDate() As String
            Get
                Return _closeDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CloseDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _closeDate.Equals(value) Then
                    _closeDate.Text = value
                    PropertyHasChanged("CloseDate")
                End If
            End Set
        End Property


        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _classHistoryId
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pClassHistoryId As Integer = ID.Trim.ToInteger
            If _classHistoryId < pClassHistoryId Then Return -1
            If _classHistoryId > pClassHistoryId Then Return 1
            Return 0
        End Function

#End Region 'Business Properties and Methods

#Region "Validation Rules"

        Private Sub AddSharedCommonRules()
            'Sample simple custom rule
            'ValidationRules.AddRule(AddressOf LDInfo.ContainsValidPeriod, "Period", 1)           

            'Sample dependent property. when check one , check the other as well
            'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")
        End Sub

        Protected Overrides Sub AddBusinessRules()
            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of CLHistory)._fieldList
                If _field.Required Then
                    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _field.FieldName, 0)
                End If
                If Not String.IsNullOrEmpty(_field.RegexPattern) Then
                    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.RegExMatch, New RegExRuleArgs(_field.FieldName, _field.RegexPattern), 1)
                End If
                '----------using lookup, if no user lookup defined, fallback to predefined by developer----------------------------
                If CATMAPInfoList.ContainsCode(_field) Then
                    ValidationRules.AddRule(AddressOf LKUInfoList.ContainsLiveCode, _field.FieldName, 2)
                    'Else
                    '    Select Case _field.FieldName
                    '        Case "LocType"
                    '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationType))
                    '        Case "Status"
                    '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationStatus))
                    '    End Select
                End If
            Next
            Rules.BusinessRules.RegisterBusinessRules(Me)
            MyBase.AddBusinessRules()
        End Sub
#End Region ' Validation

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
        End Sub

        Public Shared Function BlankCLHistory() As CLHistory
            Return New CLHistory
        End Function

        Public Shared Function NewCLHistory(ByVal pClassHistoryId As String) As CLHistory
            If KeyDuplicated(pClassHistoryId) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("CLHistory")))
            Return DataPortal.Create(Of CLHistory)(New Criteria(pClassHistoryId.ToInteger))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As CLHistory
            Dim pClassHistoryId As String = ID.Trim

            Return NewCLHistory(pClassHistoryId)
        End Function

        Public Shared Function GetCLHistory(ByVal pClassHistoryId As String) As CLHistory
            Return DataPortal.Fetch(Of CLHistory)(New Criteria(pClassHistoryId.ToInteger))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As CLHistory
            Dim pClassHistoryId As String = ID.Trim

            Return GetCLHistory(pClassHistoryId)
        End Function

        Public Shared Sub DeleteCLHistory(ByVal pClassHistoryId As String)
            DataPortal.Delete(New Criteria(pClassHistoryId.ToInteger))
        End Sub

        Public Overrides Function Save() As CLHistory
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("CLHistory")))

            Me.ApplyEdit()
            CLHistoryInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneCLHistory(ByVal pClassHistoryId As String) As CLHistory

            If CLHistory.KeyDuplicated(pClassHistoryId) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningCLHistory As CLHistory = MyBase.Clone
            cloningCLHistory._classHistoryId = pClassHistoryId.ToInteger
            cloningCLHistory._DTB = Context.CurrentBECode

            'Todo:Remember to reset status of the new object here 
            cloningCLHistory.MarkNew()
            cloningCLHistory.ApplyEdit()

            cloningCLHistory.ValidationRules.CheckRules()

            Return cloningCLHistory
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public _classHistoryId As Integer

            Public Sub New(ByVal pClassHistoryId As String)
                _classHistoryId = pClassHistoryId.ToInteger

            End Sub
        End Class

        <RunLocal()> _
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _classHistoryId = criteria._classHistoryId

            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()
                    cm.CommandType = CommandType.Text
                    cm.CommandText = <SqlText>SELECT * FROM PBS_EXT_VUS_CLHISTORY_<%= _DTB %> WHERE CLASS_HISTORY_ID= <%= criteria._classHistoryId %></SqlText>.Value.Trim

                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        If dr.Read Then
                            FetchObject(dr)
                            MarkOld()
                        End If
                    End Using

                End Using
            End Using
        End Sub

        Private Sub FetchObject(ByVal dr As SafeDataReader)
            _classHistoryId = dr.GetInt32("CLASS_HISTORY_ID")
            _changedDate.Text = dr.GetDateTime("CHANGED_DATE")
            _changedTime.Text = dr.GetDateTime("CHANGED_DATE")
            _classId.Text = dr.GetInt32("CLASS_ID")
            _classCode = dr.GetString("CLASS_CODE").TrimEnd
            _openDate.Text = dr.GetDateTime("OPEN_DATE")
            _closeDate.Text = dr.GetDateTime("CLOSE_DATE")

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionManager.GetManager
                    Using cm = ctx.Connection.CreateCommand()

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_EXT_VUS_CLHistory_{0}_InsertUpdate", _DTB)

                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As SqlCommand)
            cm.Parameters.AddWithValue("@CLASS_HISTORY_ID", _classHistoryId)
            cm.Parameters.AddWithValue("@CHANGED_DATE", String.Format("{0} {1}", _changedDate.ToString("yyyy-MM-dd"), _changedTime.ToString("HH:mm:ss")))
            cm.Parameters.AddWithValue("@CLASS_ID", _classId.DBValue)
            cm.Parameters.AddWithValue("@CLASS_CODE", _classCode.Trim)
            cm.Parameters.AddWithValue("@OPEN_DATE", _openDate.DBValue)
            cm.Parameters.AddWithValue("@CLOSE_DATE", _closeDate.DBValue)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            DataPortal_Insert()
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_classHistoryId))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()

                    cm.CommandType = CommandType.Text
                    cm.CommandText = <SqlText>DELETE PBS_EXT_VUS_CLHISTORY_<%= _DTB %> WHERE CLASS_HISTORY_ID= <%= criteria._classHistoryId %></SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        'Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
        '    If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
        '        CLHistoryInfoList.InvalidateCache()
        '    End If
        'End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pClassHistoryId As String) As Boolean
            Return CLHistoryInfoList.ContainsCode(pClassHistoryId)
        End Function

        Public Shared Function KeyDuplicated(ByVal pClassHistoryId As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM PBS_EXT_VUS_CLHISTORY_<%= Context.CurrentBECode %> WHERE CLASS_HISTORY_ID= '<%= pClassHistoryId %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneCLHistory(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(CLHistory).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(CLHistory).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return CLHistoryInfoList.GetCLHistoryInfoList
        End Function
#End Region

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _classHistoryId)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace