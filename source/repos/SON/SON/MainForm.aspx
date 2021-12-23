﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="SON.MainForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Ana Sayfa</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
   
    <style type="text/css">
        .auto-style2 {
            width: 48px;
        }
        .auto-style3 {
            height: 26px;
        }
    </style>
   
</head>
<body>
    <form id="form1" runat="server">
         
         <div>
            <asp:FileUpload  CssClass="btn btn-success"   ID="FileUpload1" runat="server" ToolTip="Select Only Excel File" />
            <br />
            <br />
            <asp:Button CssClass="btn btn-success" ID="Button1" runat="server" Text="Upload Pdf" OnClick="Button1_Click" />
            <br />
            <br />
             
            
         <asp:Button   CssClass="btn btn-dark" ID="Button2" runat="server" Text="Veri Tabanında Sorgula" OnClick="Button2_Click" />
             <br />
             <br />

             <table class="table table-bordered">
                  <thead>
                 <tr>
                     <th>Pdf İd</th>
                     <th>Pdf İsmi</th>
                     
                       <th >Kullanıcı İd</th>
                     
                      <th>Bilgiler</th>
                     
                      <th class="auto-style2">Sil</th>
                      <th>İndir</th>
                   



                 </tr>
                      </thead>
                 <asp:Repeater ID="Repeater1" runat="server">
                     <ItemTemplate>
                        
                          <tr>   

                      
                      <th ><%# Eval("İd")%></th>
                       <td ><%# Eval("FileName")%></td>
                        <td ><%# Eval("HocaİD")%></td>
                  

                 
              
                                  
                      <td><asp:HyperLink    NavigateUrl='<%#"UserPdfBilgiler.aspx?İd=" + Eval("İd") +"&Hocaİd="+Eval("Hocaİd")%>'  CssClass=" btn btn-dark" ID="HyperLink3" runat="server">Bilgiler</asp:HyperLink></td>

                   
                             
                     <td><asp:HyperLink   NavigateUrl='<%#"DeleteUserPdf.aspx?İd=" + Eval("İd") +"&Hocaİd="+Eval("Hocaİd")%>'  CssClass=" btn btn-danger" ID="HyperLink1" runat="server">Sil</asp:HyperLink></td>


                           <td> <asp:LinkButton CssClass=" btn btn-dark" ID="lnkDownload" runat="server" Text="İndir" OnClick="DownloadFile"
                    CommandArgument='<%# Eval("İd") %>'></asp:LinkButton>
                               </td> 
                   

                         </tr>
                     </ItemTemplate>
                     

                    
                 </asp:Repeater>
                
                 </table>


            

             
        </div>
         <p>
             &nbsp;</p>
         <p>
   
         </p>
    </form>
    <asp:Label ID="Label1" runat="server" Text="ABC"></asp:Label>
    </body>
</html>
