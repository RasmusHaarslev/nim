module Gui

    open System.Windows.Forms
    open System.Drawing

    let radiobuttonform=new Form(Text="Use RadioButton")
    let radio1=new RadioButton(Text="Male",Top=50,Left=90)
    let radio2=new RadioButton(Text="Female",Top=70,Left=90)

    radiobuttonform.Controls.Add(radio1)
    radiobuttonform.Controls.Add(radio2)

    Application.Run(radiobuttonform)