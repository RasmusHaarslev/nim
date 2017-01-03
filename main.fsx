#load "Gui.fsx"
#load "GameController.fsx"

open Gui
open GameController

open System.Windows.Forms
open System.Drawing

Async.StartImmediate (GameController.menu())
Application.Run(Gui.mainWindow)
