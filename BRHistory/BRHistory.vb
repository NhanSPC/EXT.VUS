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
    <DB(TableName:="pbs_EXT_VUS_BRHISTORY_{XXX}")>
    Public Class BRHistory
        Inherits Csla.BusinessBase(Of BRHistory)
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


        Private _branchHistoryId As Integer
        <System.ComponentModel.DataObjectField(True, False)> _
        <Rule(Required:=True)>
        Public ReadOnly Property BranchHistoryId() As String
            Get
                Return _branchHistoryId
            End Get
        End Property

        Private _changedDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar)>
        Public Property ChangedDate() As String
            Get
                Return String.Format("{0:yyyy-MM-dd}", _changedDate)
            End Get
            Set(value As String)
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

        Private _branchIdStudy As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public Property BranchIdStudy() As String
            Get
                Return _branchIdStudy.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("BranchIdStudy", True)
                If value Is Nothing Then value = String.Empty
                If Not _branchIdStudy.Equals(value) Then
                    _branchIdStudy.Text = value
                    PropertyHasChanged("BranchIdStudy")
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

        Private _operationId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public Property OperationId() As String
            Get
                Return _operationId.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("OperationId", True)
                If value Is Nothing Then value = String.Empty
                If Not _operationId.Equals(value) Then
                    _operationId.Text = value
                    PropertyHasChanged("OperationId")
                End If
            End Set
        End Property


        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _branchHistoryId
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pBranchHistoryId As Integer = ID.Trim.ToInteger
            If _branchHistoryId < pBranchHistoryId Then Return -1
            If _branchHistoryId > pBranchHistoryId Then Return 1
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

            For Each _field As ClassField In ClassSchema(Of BRHistory)._fieldList
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

        Public Shared Function BlankBRHistory() As BRHistory
            Return New BRHistory
        End Function

        Public Shared Function NewBRHistory(ByVal pBranchHistoryId As String) As BRHistory
            If KeyDuplicated(pBranchHistoryId) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("BRHistory")))
            Return DataPortal.Create(Of BRHistory)(New Criteria(pBranchHistoryId.ToInteger))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As BRHistory
            Dim pBranchHistoryId As String = ID.Trim

            Return NewBRHistory(pBranchHistoryId)
        End Function

        Public Shared Function GetBRHistory(ByVal pBranchHistoryId As String) As BRHistory
            Return DataPortal.Fetch(Of BRHistory)(New Criteria(pBranchHistoryId.ToInteger))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As BRHistory
            Dim pBranchHistoryId As String = ID.Trim

            Return GetBRHistory(pBranchHistoryId)
        End Function

        Public Shared Sub DeleteBRHistory(ByVal pBranchHistoryId As String)
            DataPortal.Delete(New Criteria(pBranchHistoryId.ToInteger))
        End Sub

        Public Overrides Function Save() As BRHistory
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("BRHistory")))

            Me.ApplyEdit()
            BRHistoryInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneBRHistory(ByVal pBranchHistoryId As String) As BRHistory

            If BRHistory.KeyDuplicated(pBranchHistoryId) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningBRHistory As BRHistory = MyBase.Clone
            cloningBRHistory._branchHistoryId = pBranchHistoryId.ToInteger
            cloningBRHistory._DTB = Context.CurrentBECode

            'Todo:Remember to reset status of the new object here 
            cloningBRHistory.MarkNew()
            cloningBRHistory.ApplyEdit()

            cloningBRHistory.ValidationRules.CheckRules()

            Return cloningBRHistory
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public _branchHistoryId As Integer

            Public Sub New(ByVal pBranchHistoryId As String)
                _branchHistoryId = pBranchHistoryId.ToInteger

            End Sub
        End Class

        <RunLocal()> _
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _branchHistoryId = criteria._branchHistoryId

            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()
                    cm.CommandType = CommandType.Text
                    cm.CommandText = <SqlText>SELECT * FROM PBS_EXT_VUS_BRHISTORY_<%= _DTB %> WHERE BRANCH_HISTORY_ID= <%= criteria._branchHistoryId %></SqlText>.Value.Trim

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
            _branchHistoryId = dr.GetInt32("BRANCH_HISTORY_ID")
            _changedDate.Text = dr.GetDateTime("CHANGED_DATE")
            _changedTime.Text = dr.GetDateTime("CHANGED_DATE")
            _branchIdStudy.Text = dr.GetInt32("BRANCH_ID_STUDY")
            _classId.Text = dr.GetInt32("CLASS_ID")
            _operationId.Text = dr.GetInt32("OPERATION_ID")

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionManager.GetManager
                    Using cm = ctx.Connection.CreateCommand()

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_EXT_VUS_BRHistory_{0}_InsertUpdate", _DTB)

                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As SqlCommand)
            cm.Parameters.AddWithValue("@BRANCH_HISTORY_ID", _branchHistoryId)
            'cm.Parameters.AddWithValue("@CHANGED_DATE", If(_changedDate.Year = 1, DBNull.Value, _changedDate))
            cm.Parameters.AddWithValue("CHANGED_DATE", String.Format("{0} {1}", _changedDate.ToString("yyyy-MM-dd"), _changedTime.ToString("HH:mm:ss")))
            cm.Parameters.AddWithValue("@BRANCH_ID_STUDY", _branchIdStudy.DBValue)
            cm.Parameters.AddWithValue("@CLASS_ID", _classId.DBValue)
            cm.Parameters.AddWithValue("@OPERATION_ID", _operationId.DBValue)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            DataPortal_Insert()
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_branchHistoryId))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()

                    cm.CommandType = CommandType.Text
                    cm.CommandText = <SqlText>DELETE PBS_EXT_VUS_BRHISTORY_<%= _DTB %> WHERE BRANCH_HISTORY_ID= <%= criteria._branchHistoryId %></SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        'Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
        '    If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
        '        BRHistoryInfoList.InvalidateCache()
        '    End If
        'End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pBranchHistoryId As String) As Boolean
            Return BRHistoryInfoList.ContainsCode(pBranchHistoryId)
        End Function

        Public Shared Function KeyDuplicated(ByVal pBranchHistoryId As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM PBS_EXT_VUS_BRHISTORY_<%= Context.CurrentBECode %> WHERE BRANCH_HISTORY_ID= '<%= pBranchHistoryId %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneBRHistory(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(BRHistory).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(BRHistory).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return BRHistoryInfoList.GetBRHistoryInfoList
        End Function
#End Region

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _branchHistoryId)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace