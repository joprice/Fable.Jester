module UserEventTests

open Browser.Blob
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Fable.Jester
open Fable.ReactTestingLibrary
open Feliz

let inputTestElement = React.functionComponent(fun () ->
    let value, setValue = React.useState "Hello"

    Html.div [
        Html.input [
            prop.type'.text
            prop.testId "test-input"
            prop.onChange setValue
        ]
        Html.h1 [
            prop.text value
            prop.testId "header"
        ]
    ])

let selectMultipleTestElement = React.functionComponent(fun () ->
    Html.select [
        prop.testId "test-select-multiple"
        prop.multiple true

        prop.children [
            Html.option [
                prop.testId "val1"
                prop.value 1
                prop.text "A"
            ]
            Html.option [
                prop.testId "val2"
                prop.value 2
                prop.text "B"
            ]
            Html.option [
                prop.testId "val3"
                prop.value 3
                prop.text "C"
            ]
        ]
    ])

let textAreaTestElement = React.functionComponent(fun () ->
    Html.textarea [
        prop.testId "test-textarea"
    ])

let buttonTestElement = React.functionComponent(fun () ->
    let value, setValue = React.useState "Hello"

    Html.div [
        Html.button [
            prop.testId "test-button"
            prop.onClick (fun _ -> setValue "Howdy!")
            prop.onDoubleClick (fun _ -> setValue "Bonjour!")
        ]
        Html.h1 [
            prop.text value
            prop.testId "header"
        ]
    ])

let uploadTestElement = React.functionComponent(fun (input: {| isMultiple: bool |}) ->
    Html.input [
        prop.type'.file
        prop.testId "upload"
        prop.multiple input.isMultiple
    ])

let hoverTestElement = React.functionComponent(fun () ->
    let hover,setHover = React.useState false
    
    Html.div [
        prop.testId "test-hover"

        prop.onMouseEnter(fun _ -> setHover true)
        prop.onMouseLeave(fun _ -> setHover false)
        
        prop.text (string hover)
    ])

Jest.describe("UserEvent tests", fun () ->
    Jest.test("dispatch input change", promise {
        let elem = RTL.render(inputTestElement()).getByTestId("test-input")

        do! elem.userEvent.type'("Hello world")

        Jest.expect(RTL.screen.getByTestId("header")).toHaveTextContent("Hello world")
    })
    Jest.test("dispatch input change", promise{
        let elem = RTL.render(inputTestElement()).getByTestId("test-input")

        do! elem.userEvent.type'("Hello world")

        Jest.expect(RTL.screen.getByTestId("header")).not.toHaveTextContent("somethingElse")
    })

    Jest.test("dispatch textarea change", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.type'("Hello{enter}world")

        Jest.expect(RTL.screen.getByTestId("test-textarea")).toHaveValue("Hello\nworld")
    })
    Jest.test("dispatch textarea change", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.type'("Hello{enter}world")

        Jest.expect(RTL.screen.getByTestId("test-textarea")).not.toHaveValue("somethingElse")
    })

    Jest.test("dispatch textarea change via keyboard", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.click()

        do! RTL.userEvent.keyboard("Hello{enter}world")

        Jest.expect(RTL.screen.getByTestId("test-textarea")).toHaveValue("Hello\nworld")
    })
    Jest.test("dispatch textarea change via keyboard", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.click()

        do! RTL.userEvent.keyboard("Hello{enter}world")

        Jest.expect(RTL.screen.getByTestId("test-textarea")).not.toHaveValue("somethingElse")
    })

    Jest.test("dispatch textarea change via keyboard async", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.click()

        do! RTL.userEvent.keyboard("Hello{enter}world", 100)

        return Jest.expect(RTL.screen.getByTestId("test-textarea")).toHaveValue("Hello\nworld")
    })
    Jest.test("dispatch textarea change via keyboard async", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.click()

        do! RTL.userEvent.keyboard("Hello{enter}world", 100)

        return Jest.expect(RTL.screen.getByTestId("test-textarea")).not.toHaveValue("somethingElse")
    })

    Jest.test("dispatch textarea change via keyboard with state", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.click()

        do! RTL.userEvent.keyboard("hello")

        do! RTL.userEvent.keyboard("{Enter}world")

        Jest.expect(RTL.screen.getByTestId("test-textarea")).toHaveValue("hello\nworld")
    })
    Jest.test("dispatch textarea change via keyboard with state", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.click()

        let ks = RTL.userEvent.keyboardWithState("hello")

        do! RTL.userEvent.keyboard("{Enter}world")

        Jest.expect(RTL.screen.getByTestId("test-textarea")).not.toHaveValue("somethingElse")
    })

    Jest.test("dispatch textarea change via keyboard with state async", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.click()

        let ks = RTL.userEvent.keyboardWithState("hello")

        do! RTL.userEvent.keyboard("{Enter}world", 100)

        return Jest.expect(RTL.screen.getByTestId("test-textarea")).toHaveValue("hello\nworld")
    })
    Jest.test("dispatch textarea change via keyboard with state async", promise {
        let elem = RTL.render(textAreaTestElement()).getByTestId("test-textarea")

        do! elem.userEvent.click()

        let! ks = RTL.userEvent.keyboardWithState("hello", 100)

        do! RTL.userEvent.keyboard("{Enter}world", 100, keyboardState = ks)

        return Jest.expect(RTL.screen.getByTestId("test-textarea")).not.toHaveValue("somethingElse")
    })

    Jest.test("clear input element", promise {
        let elem = RTL.render(inputTestElement()).getByTestId("test-input")

        do! elem.userEvent.type'("Hello world")

        Jest.expect(RTL.screen.getByTestId("header")).toHaveTextContent("Hello world")

        do! elem.userEvent.clear()

        Jest.expect(RTL.screen.getByTestId("test-input")).toBeEmptyDOMElement()
    })

    Jest.test("dispatch button click", promise {
        let elem = RTL.render(buttonTestElement()).getByTestId("test-button")

        do! elem.userEvent.click()

        Jest.expect(RTL.screen.getByTestId("header")).toHaveTextContent("Howdy!")
    })
    Jest.test("dispatch button click", promise {
        let elem = RTL.render(buttonTestElement()).getByTestId("test-button")

        do! elem.userEvent.click()

        Jest.expect(RTL.screen.getByTestId("header")).not.toHaveTextContent("somethingElse")
    })

    // Jest.test("dispatch button double click", fun () ->
    //     let elem = RTL.render(buttonTestElement()).getByTestId("test-button")
    //
    //     elem.userEvent.dblClick()
    //
    //     Jest.expect(RTL.screen.getByTestId("header")).toHaveTextContent("Bonjour!")
    // )

    Jest.test("dispatch button double click 2", promise {
        let elem = RTL.render(buttonTestElement()).getByTestId("test-button")

        do! elem.userEvent.dblClick()

        Jest.expect(RTL.screen.getByTestId("header")).not.toHaveTextContent("somethingElse")
    })

    Jest.test("dispatch selection of options", promise {
        let elem = RTL.render(selectMultipleTestElement()).getByTestId("test-select-multiple")

        do! elem.userEvent.selectOptions(["1";"3"])

        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val1")).selected).toEqual(true)
        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val2")).selected).toEqual(false)
        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val3")).selected).toEqual(true)
    })

    Jest.test("dispatch de-selection of options", promise {
        let elem = RTL.render(selectMultipleTestElement()).getByTestId("test-select-multiple")

        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val1")).selected).toEqual(false)
        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val2")).selected).toEqual(false)
        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val3")).selected).toEqual(false)

        do! elem.userEvent.selectOptions(["1";"3"])

        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val1")).selected).toEqual(true)
        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val2")).selected).toEqual(false)
        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val3")).selected).toEqual(true)

        do! elem.userEvent.deselectOptions(["1"])

        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val1")).selected).toEqual(false)
        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val2")).selected).toEqual(false)
        Jest.expect((RTL.screen.getByTestId<HTMLOptionElement>("val3")).selected).toEqual(true)
    })

    Jest.test("can upload a file", promise {
        let elem : HTMLInputElement = RTL.render(uploadTestElement {| isMultiple = false |}).getByTestId("upload") |> unbox

        let myFile = 
            let propBag = 
                JsInterop.jsOptions<BlobPropertyBag>(fun o -> o.``type`` <- "image/png")

            let file = Blob.Create([| "hello" :> obj |], propBag) :?> File

            file?name <- "hello.png"

            file

        do! elem.userEvent.upload(myFile)

        Jest.expect(elem.files.[0]).toStrictEqual(myFile)
        Jest.expect(elem.files.item(0)).toStrictEqual(myFile)
        Jest.expect(elem.files).toHaveLength(1)
    })
    Jest.test("can upload multiple files", promise {
        let elem : HTMLInputElement = RTL.render(uploadTestElement {| isMultiple = true |}).getByTestId("upload") |> unbox

        let myFiles = 
            let propBag = 
                JsInterop.jsOptions<BlobPropertyBag>(fun o -> o.``type`` <- "image/png")

            let file = 
                Blob.Create([| "hello" :> obj |], propBag) :?> File
                |> fun file -> 
                    file?name <- "hello.png"
                    file

            let file2 =
                Blob.Create([| "there" :> obj |], propBag) :?> File
                |> fun file -> 
                    file?name <- "there.png"
                    file

            [ file; file2 ]

        do! elem.userEvent.upload(myFiles)

        Jest.expect(elem.files.[0]).toStrictEqual(myFiles.[0])
        Jest.expect(elem.files.[1]).toStrictEqual(myFiles.[1])
        Jest.expect(elem.files).toHaveLength(2)
    })

    Jest.test("can hover an element", promise {
        let elem = RTL.render(hoverTestElement()).getByTestId "test-hover"

        Jest.expect(elem).toHaveTextContent "false"

        do! elem.userEvent.hover()

        Jest.expect(elem).toHaveTextContent "true"

        do! elem.userEvent.unhover()

        Jest.expect(elem).toHaveTextContent "false"
    })
)
