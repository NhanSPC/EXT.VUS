﻿
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace EXT.VUS

    <Serializable()> _
    Public Class REVInfo
        Inherits Csla.ReadOnlyBase(Of REVInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        'Implements IInfoStatus


#Region " Business Properties and Methods "


        Private _operationId As Integer
        Public ReadOnly Property OperationId() As String
            Get
                Return _operationId
            End Get
        End Property

        Private _branchIdStudy As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property BranchIdStudy() As String
            Get
                Return _branchIdStudy.Text
            End Get
        End Property

        Private _branchIdPayment As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property BranchIdPayment() As String
            Get
                Return _branchIdPayment.Text
            End Get
        End Property

        Private _invoiceNo As String = String.Empty
        Public ReadOnly Property InvoiceNo() As String
            Get
                Return _invoiceNo
            End Get
        End Property

        Private _invoiceNumber As String = String.Empty
        Public ReadOnly Property InvoiceNumber() As String
            Get
                Return _invoiceNumber
            End Get
        End Property

        Private _invoiceDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property InvoiceDate() As String
            Get
                Return _invoiceDate.DateViewFormat
            End Get
        End Property

        Private _totalPayment As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property TotalPayment() As String
            Get
                Return _totalPayment.Text
            End Get
        End Property

        Private _classId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property ClassId() As String
            Get
                Return _classId.Text
            End Get
        End Property

        Private _createdOn As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property CreatedOn() As String
            Get
                Return _createdOn.DateViewFormat
            End Get
        End Property

        Private _createdBy As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property CreatedBy() As String
            Get
                Return _createdBy.Text
            End Get
        End Property

        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _operationId
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pOperationId As Integer = ID.Trim.ToInteger
            If _operationId < pOperationId Then Return -1
            If _operationId > pOperationId Then Return 1
            Return 0
        End Function

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _operationId
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _operationId
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _operationId
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            REVInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetREVInfo(ByVal dr As SafeDataReader) As REVInfo
            Return New REVInfo(dr)
        End Function

        Friend Shared Function EmptyREVInfo(Optional ByVal pOperationId As String = "") As REVInfo
            Dim info As REVInfo = New REVInfo
            With info
                ._operationId = pOperationId.ToInteger

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _operationId = dr.GetInt32("OPERATION_ID")
            _branchIdStudy.Text = dr.GetInt32("BRANCH_ID_STUDY")
            _branchIdPayment.Text = dr.GetInt32("BRANCH_ID_PAYMENT")
            _invoiceNo = dr.GetString("INVOICE_NO").TrimEnd
            _invoiceNumber = dr.GetString("INVOICE_NUMBER").TrimEnd
            _invoiceDate.Text = dr.GetDateTime("INVOICE_DATE")
            _totalPayment.Text = dr.GetDecimal("TOTAL_PAYMENT")
            _classId.Text = dr.GetInt32("CLASS_ID")
            _createdOn.Text = dr.GetDateTime("CREATED_ON")
            _createdBy.Text = dr.GetInt32("CREATED_BY")

        End Sub

        Private Sub New()
        End Sub


#End Region ' Factory Methods

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _operationId)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace