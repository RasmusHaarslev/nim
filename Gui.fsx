module Gui

    open System.Windows.Forms
    open System.Drawing

    let mainWindow = new Form(Text="Nim Player",Size=Size(520,500))
    mainWindow.FormBorderStyle <- FormBorderStyle.FixedSingle

    let heapPanel = new Panel(Size=Size(500,400),Top=10,Left=10)
    heapPanel.AutoScroll <- true
    heapPanel.BackColor <- Color.Gray

    let mutable i = 20
    while i < 20*30 do
        let rad = new RadioButton(Text=i.ToString(),Top=i,Left=30)
        heapPanel.Controls.Add rad
        i <- i + 20

    let newGameButton =
      new Button(Location=Point(30,420), MinimumSize=Size(100,50),
                  MaximumSize=Size(100,50),Text="New Game")

    let makeDrawButton =
      new Button(Location=Point(380,420), MinimumSize=Size(100,50),
                  MaximumSize=Size(100,50),Text="Draw!")

    let chooseMatches = 
      new TextBox(Location=Point(150,420),Size=Size(200,25))

    mainWindow.Controls.Add newGameButton
    mainWindow.Controls.Add newGameButton
    mainWindow.Controls.Add makeDrawButton
    mainWindow.Controls.Add heapPanel

    