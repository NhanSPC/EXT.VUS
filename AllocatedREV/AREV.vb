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
    Public Class AREV
        Inherits Csla.BusinessBase(Of AREV)
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


        Private _lineNo As Integer
        <System.ComponentModel.DataObjectField(True, True)> _
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
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

        Private _branchIdPayment As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public Property BranchIdPayment() As String
            Get
                Return _branchIdPayment.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("BranchIdPayment", True)
                If value Is Nothing Then value = String.Empty
                If Not _branchIdPayment.Equals(value) Then
                    _branchIdPayment.Text = value
                    PropertyHasChanged("BranchIdPayment")
                End If
            End Set
        End Property

        Private _invoiceNo As String = String.Empty
        Public Property InvoiceNo() As String
            Get
                Return _invoiceNo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceNo.Equals(value) Then
                    _invoiceNo = value
                    PropertyHasChanged("InvoiceNo")
                End If
            End Set
        End Property

        Private _invoiceNumber As String = String.Empty
        Public Property InvoiceNumber() As String
            Get
                Return _invoiceNumber
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceNumber", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceNumber.Equals(value) Then
                    _invoiceNumber = value
                    PropertyHasChanged("InvoiceNumber")
                End If
            End Set
        End Property

        Private _invoiceDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public Property InvoiceDate() As String
            Get
                Return _invoiceDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceDate.Equals(value) Then
                    _invoiceDate.Text = value
                    PropertyHasChanged("InvoiceDate")
                End If
            End Set
        End Property

        Private _period As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public Property Period() As String
            Get
                Return _period.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Period", True)
                If value Is Nothing Then value = String.Empty
                If Not _period.Equals(value) Then
                    _period.Text = value
                    PropertyHasChanged("Period")
                End If
            End Set
        End Property

        Private _amount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public Property Amount() As String
            Get
                Return _amount.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Amount", True)
                If value Is Nothing Then value = String.Empty
                If Not _amount.Equals(value) Then
                    _amount.Text = value
                    PropertyHasChanged("Amount")
                End If
            End Set
        End Property

        Private _totalPayment As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public Property TotalPayment() As String
            Get
                Return _totalPayment.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TotalPayment", True)
                If value Is Nothing Then value = String.Empty
                If Not _totalPayment.Equals(value) Then
                    _totalPayment.Text = value
                    PropertyHasChanged("TotalPayment")
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

        Private _createdOn As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public Property CreatedOn() As String
            Get
                Return _createdOn.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CreatedOn", True)
                If value Is Nothing Then value = String.Empty
                If Not _createdOn.Equals(value) Then
                    _createdOn.Text = value
                    PropertyHasChanged("CreatedOn")
                End If
            End Set
        End Property

        Private _createdBy As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public Property CreatedBy() As String
            Get
                Return _createdBy.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CreatedBy", True)
                If value Is Nothing Then value = String.Empty
                If Not _createdBy.Equals(value) Then
                    _createdBy.Text = value
                    PropertyHasChanged("CreatedBy")
                End If
            End Set
        End Property


        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _lineNo
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pLineNo As Integer = ID.Trim.ToInteger
            If _lineNo < pLineNo Then Return -1
            If _lineNo > pLineNo Then Return 1
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

            For Each _field As ClassField In ClassSchema(Of AREV)._fieldList
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

        Public Shared Function BlankAREV() As AREV
            Return New AREV
        End Function

        Public Shared Function NewAREV(ByVal pLineNo As String) As AREV
            'If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("AREV")))
            Return DataPortal.Create(Of AREV)(New Criteria(pLineNo.ToInteger))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As AREV
            Dim pLineNo As String = ID.Trim

            Return NewAREV(pLineNo)
        End Function

        Public Shared Function GetAREV(ByVal pLineNo As String) As AREV
            Return DataPortal.Fetch(Of AREV)(New Criteria(pLineNo.ToInteger))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As AREV
            Dim pLineNo As String = ID.Trim

            Return GetAREV(pLineNo)
        End Function

        Public Shared Sub DeleteAREV(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As AREV
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("AREV")))

            Me.ApplyEdit()
            AREVInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneAREV(ByVal pLineNo As String) As AREV

            'If AREV.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningAREV As AREV = MyBase.Clone
            cloningAREV._lineNo = 0
            cloningAREV._DTB = Context.CurrentBECode

            'Todo:Remember to reset status of the new object here 
            cloningAREV.MarkNew()
            cloningAREV.ApplyEdit()

            cloningAREV.ValidationRules.CheckRules()

            Return cloningAREV
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public _lineNo As Integer

            Public Sub New(ByVal pLineNo As String)
                _lineNo = pLineNo.ToInteger

            End Sub
        End Class

        <RunLocal()> _
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _lineNo = criteria._lineNo

            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()
                    cm.CommandType = CommandType.Text
                    cm.CommandText = <SqlText>SELECT * FROM PBS_EXT_VUS_ALLOCATED_REV_<%= _DTB %> WHERE LINE_NO= <%= criteria._lineNo %></SqlText>.Value.Trim

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
            _lineNo = dr.GetInt32("LINE_NO")
            _operationId.Text = dr.GetInt32("OPERATION_ID")
            _branchIdStudy.Text = dr.GetInt32("BRANCH_ID_STUDY")
            _branchIdPayment.Text = dr.GetInt32("BRANCH_ID_PAYMENT")
            _invoiceNo = dr.GetString("INVOICE_NO").TrimEnd
            _invoiceNumber = dr.GetString("INVOICE_NUMBER").TrimEnd
            _invoiceDate.Text = dr.GetDateTime("INVOICE_DATE")
            _period.Text = dr.GetInt32("PERIOD")
            _amount.Text = dr.GetDecimal("AMOUNT")
            _totalPayment.Text = dr.GetDecimal("TOTAL_PAYMENT")
            _classId.Text = dr.GetInt32("CLASS_ID")
            _createdOn.Text = dr.GetDateTime("CREATED_ON")
            _createdBy.Text = dr.GetInt32("CREATED_BY")

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionManager.GetManager
                    Using cm = ctx.Connection.CreateCommand()

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_EXT_VUS_ALLOCATED_REV_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo).Direction = ParameterDirection.Output
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = CInt(cm.Parameters("@LINE_NO").Value)
                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As SqlCommand)
            cm.Parameters.AddWithValue("@OPERATION_ID", _operationId.DBValue)
            cm.Parameters.AddWithValue("@BRANCH_ID_STUDY", _branchIdStudy.DBValue)
            cm.Parameters.AddWithValue("@BRANCH_ID_PAYMENT", _branchIdPayment.DBValue)
            cm.Parameters.AddWithValue("@INVOICE_NO", _invoiceNo.Trim)
            cm.Parameters.AddWithValue("@INVOICE_NUMBER", _invoiceNumber.Trim)
            cm.Parameters.AddWithValue("@INVOICE_DATE", _invoiceDate.DBValue)
            cm.Parameters.AddWithValue("@PERIOD", _period.DBValue)
            cm.Parameters.AddWithValue("@AMOUNT", _amount.DBValue)
            cm.Parameters.AddWithValue("@TOTAL_PAYMENT", _totalPayment.DBValue)
            cm.Parameters.AddWithValue("@CLASS_ID", _classId.DBValue)
            cm.Parameters.AddWithValue("@CREATED_ON", _createdOn.DBValue)
            cm.Parameters.AddWithValue("@CREATED_BY", _createdBy.DBValue)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionManager.GetManager
                    Using cm = ctx.Connection.CreateCommand()

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_EXT_VUS_ALLOCATED_REV_{0}_Update", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                    End Using
                End Using
            End SyncLock
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_lineNo))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()

                    cm.CommandType = CommandType.Text
                    cm.CommandText = <SqlText>DELETE PBS_EXT_VUS_ALLOCATED_REV_<%= _DTB %> WHERE LINE_NO= <%= criteria._lineNo %></SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        'Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
        '    If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
        '        AREVInfoList.InvalidateCache()
        '    End If
        'End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return AREVInfoList.ContainsCode(pLineNo)
        End Function

        'Public Shared Function KeyDuplicated(ByVal pLineNo As SmartInt32) As Boolean
        '    Dim SqlText = <SqlText>SELECT COUNT(*) FROM PBS_EXT_VUS_ALLOCATED_REV_DEM WHERE DTB='<%= Context.CurrentBECode %>'  AND LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
        '    Return SQLCommander.GetScalarInteger(SqlText) > 0
        'End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneAREV(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(AREV).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(AREV).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return AREVInfoList.GetAREVInfoList
        End Function
#End Region

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _lineNo)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace