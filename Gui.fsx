module Gui

    open System.Windows.Forms
    open System.Drawing

    let mainWindow = new Form(Text="Nim Player",Size=Size(520,500))
    // Next line disables resizing of the window
    mainWindow.FormBorderStyle <- FormBorderStyle.FixedSingle

    // Add a panel, to group up all the radio buttons
    let heapPanel = new Panel(Size=Size(500,400),Top=10,Left=10)
    heapPanel.AutoScroll <- true
    heapPanel.BackColor <- Color.Gray

    // This is sort of temporary. We need to add radiobuttons,
    // dynamically, based on the amount of heaps.
    // This loops adds radiobuttons to the panel we made  before.
    //let mutable i = 20
    //while i < 20*30 do
    //    let rad = new RadioButton(Text=i.ToString(),Top=i,Left=30)
    //    heapPanel.Controls.Add rad
    //    i <- i + 20

    // Add the buttons/text fields in the bottom of the screen.
    let newGameButton =
      new Button(Location=Point(30,420), MinimumSize=Size(100,50),
                  MaximumSize=Size(100,50),Text="New Game")

    let makeDrawButton =
      new Button(Location=Point(380,420), MinimumSize=Size(100,50),
                  MaximumSize=Size(100,50),Text="Draw!")

    let chooseMatches = 
      new TextBox(Location=Point(150,420),Size=Size(200,25))


    let populateHeapPanel heaps =
        //let mutable i = 1
        //let heapNum = List.length heaps
        //while i <= heapNum do
        let rec inner heaps yPos = 
            match heaps with
            | []        -> ()
            | x::xs     ->
                let rad = new RadioButton(Text=x.ToString(),Top=yPos,Left=30)
                heapPanel.Controls.Add rad
                inner xs (yPos + 20)

        in
            inner heaps 0
            


    // Add all the items.
    mainWindow.Controls.Add newGameButton
    mainWindow.Controls.Add chooseMatches
    mainWindow.Controls.Add makeDrawButton

    populateHeapPanel [222;23234;1111;23;5]

    mainWindow.Controls.Add heapPanel

    