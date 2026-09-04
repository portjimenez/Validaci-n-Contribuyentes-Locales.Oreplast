<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contribuyentes.aspx.cs" Inherits="identificadores_de_contribuyentes.Datos_contribuyentes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Contribuyentes Locales</title>
    <!-- Agregar la referencia a Bootstrap -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="shortcut icon" href="Oreplast.ico" />
    <style>
        .label-right {
            text-align: right;
            display: block;
        }
        .form-group {
            display: flex;
            flex-direction: column;
            align-items: flex-end;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="row">
                <div class="col-12 text-center">
                    <asp:Label ID="Label1" runat="server" CssClass="h3 font-weight-bold" Text="Validación de Contribuyentes Locales"></asp:Label>
                </div>
            </div>
            <div class="row mt-4 justify-content-center">
                <div class="col-md-6">
                    <asp:Label ID="Label2" runat="server" CssClass="form-label label-left" Text="Digite el CUI o el NIT a verificar sin espacios" AssociatedControlID="CUIorNIT"></asp:Label>
                    <div class="form-group">
                        <asp:TextBox ID="CUIorNIT" runat="server" CssClass="form-control mt-2 text-center" OnTextChanged="CUIorNIT_TextChanged"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row mt-4">
                <div class="col-12 text-center">
                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="Button1_Click1" />
                </div>
            </div>

            <div class="row mt-4">
                 <div  class="col-12 text-center">
                    <p>
                        <asp:Label ID="Label3" runat="server" Text="Label" Visible="False"></asp:Label>
                    </p>
                    <p>
                        <asp:Label ID="Label4" runat="server" Text="Label" Visible="False"></asp:Label>
                    </p>
                    <p>
                        <asp:Label ID="Label5" runat="server" Text="Label" Visible="False"></asp:Label>
                    </p>
                        <asp:Label ID="Label6" runat="server" Text="Label" Visible="False"></asp:Label>
                    <p>
                        <asp:Label ID="Label7" runat="server" Text="Label" Visible="False"></asp:Label>
                    </p>
                </div>
            </div>
        </div>
        

    <!-- Agregar el script de Bootstrap -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    </form>
    </body>
</html>
