<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Mensaje.aspx.vb" Inherits="Formulario_Consulta.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title> </title>
    <style type="text/css">
        #Submit1 {
            height: 26px;
            width: 49px;
        }
        #form1 {
            height: 148px;
        }
    </style>
</head>
<body id="Body" runat="server">
     <form id="form1" runat="server">
       <div style="z-index: 103; left: 10px; width: 907px; position: absolute; top: 10px;
                height: 602px; overflow: auto;">
                    <asp:GridView ID="GridMensaje" runat="server" Height="12px" Style="z-index: 900; left: 0px;
                    top: 0px; margin-top: 6px;" Width="896px" AutoGenerateColumns="False" Font-Size="Small" CellPadding="4"
                        ForeColor="#333333" GridLines="None" TabIndex="100">
                        <Columns>
                        <asp:TemplateField HeaderText="Mensaje">
                            <ItemTemplate>
                                <asp:Label ID="Label2_3" runat="server" Text='<%# Bind("Mensaje") %>'></asp:Label><br />
                            </ItemTemplate>
                        </asp:TemplateField>
                                    
                        </Columns>
                        <RowStyle BackColor="#EFF3FB" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    &nbsp;
                </div>
   </form>

 </body>


</html>
