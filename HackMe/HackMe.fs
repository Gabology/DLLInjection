module HackMe
open System
open System.Windows.Forms


let getMsgBoxTxt() = "No one has hacked me yet!"

type MainForm() =
    let form = new Form(Width = 100, Height = 100)
    let btn = new Button(Text = "Click me!", Left = 80, Top = 20)

    do
        btn.Click.Add(fun _ ->
            getMsgBoxTxt() |> MessageBox.Show |> ignore)
        form.Controls.Add(btn)

    member __.Render() =
        Application.Run(form)

let mainForm = MainForm()

[<EntryPoint; STAThread>]
let main argv =
    Application.EnableVisualStyles()
    mainForm.Render()
    0
