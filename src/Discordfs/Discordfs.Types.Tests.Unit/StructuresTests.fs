namespace Discordfs.Types

open Microsoft.VisualStudio.TestTools.UnitTesting
open Discordfs.Types
open Discordfs.Types.Utils
open System.Text.Json

[<TestClass>]
type StructuresTests () =
    [<TestMethod>]
    member _.ComponentConverter_Read_CorrectlyDeserializesActionRow () =
        // Arrange
        let json = """{"type":1,"components":[]}"""

        let expected = Component.ActionRow {
            Type = ComponentType.ACTION_ROW;
            Components = [];
        }

        // Act
        let actual = FsJson.deserialize<Component> json

        // Assert
        Assert.AreEqual<Component>(expected, actual)

    [<TestMethod>]
    member _.ComponentConverter_Read_CorrectlyDeserializesButton () =
        // Arrange
        let json = """{"type":2,"style":1,"label":"Label","emoji":null,"custom_id":"custom-id","url":null,"disabled":null}"""

        let expected = Component.Button {
            Type = ComponentType.BUTTON;
            Style = ButtonStyle.PRIMARY;
            Label = "Label";
            Emoji = None;
            CustomId = Some "custom-id";
            Url = None;
            Disabled = None;
        }

        // Act
        let actual = FsJson.deserialize<Component> json

        // Assert
        Assert.AreEqual<Component>(expected, actual)
        
    [<TestMethod>]
    member _.ComponentConverter_Read_CorrectlyDeserializesSelectMenu () =
        // Arrange
        let json = """{"type":3,"custom_id":"custom-id","options":[],"channel_types":null,"placeholder":null,"default_values":null,"min_values":null,"max_values":null,"disabled":null}"""
        
        let expected = Component.SelectMenu {
            Type = ComponentType.STRING_SELECT;
            CustomId = "custom-id";
            Options = Some [];
            ChannelTypes = None;
            Placeholder = None;
            DefaultValues = None;
            MinValues = None;
            MaxValues = None;
            Disabled = None;
        }

        // Act
        let actual = FsJson.deserialize<Component> json

        // Assert
        Assert.AreEqual<Component>(expected, actual)
        
    [<TestMethod>]
    member _.ComponentConverter_Read_CorrectlyDeserializesTextInput () =
        // Arrange
        let json = """{"type":4,"custom_id":"custom-id","style":1,"label":"Label","min_length":null,"max_length":null,"required":null,"value":null,"placeholder":null}"""
        
        let expected = Component.TextInput {
            Type = ComponentType.TEXT_INPUT;
            CustomId = "custom-id";
            Style = TextInputStyle.SHORT;
            Label = "Label";
            MinLength = None;
            MaxLength = None;
            Required = None;
            Value = None;
            Placeholder = None;
        }

        // Act
        let actual = FsJson.deserialize<Component> json

        // Assert
        Assert.AreEqual<Component>(expected, actual)

    [<TestMethod>]
    member _.ComponentConverter_Read_FailsOnInvalidComponentType () =
        // Arrange
        let json = """{"type":0}"""

        // Act
        let res () = FsJson.deserialize<Component> json |> ignore

        // Assert
        Assert.ThrowsException res |> ignore

    [<TestMethod>]
    member _.ComponentConverter_Read_FailsOnInvalidJsonString () =
        // Arrange
        let json = """{this is in valid json}"""

        // Act
        let res () = FsJson.deserialize<Component> json |> ignore

        // Assert
        Assert.ThrowsException<JsonException> res |> ignore

    [<TestMethod>]
    member _.ComponentConverter_Write_CorrectlySerializesActionRow () =
        // Arrange
        let actionRow = Component.ActionRow {
            Type = ComponentType.ACTION_ROW;
            Components = [];
        }

        let expected = """{"type":1,"components":[]}"""

        // Act
        let actual = FsJson.serialize actionRow

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    member _.ComponentConverter_Write_CorrectlySerializesButton () =
        // Arrange
        let button = Component.Button {
            Type = ComponentType.BUTTON;
            Style = ButtonStyle.PRIMARY;
            Label = "Label";
            Emoji = None;
            CustomId = Some "custom-id";
            Url = None;
            Disabled = None;
        }

        let expected = """{"type":2,"style":1,"label":"Label","emoji":null,"custom_id":"custom-id","url":null,"disabled":null}"""

        // Act
        let actual = FsJson.serialize button

        // Assert
        Assert.AreEqual<string>(expected, actual)
        
    [<TestMethod>]
    member _.ComponentConverter_Write_CorrectlySerializesSelectMenu () =
        // Arrange
        let selectMenu = Component.SelectMenu {
            Type = ComponentType.STRING_SELECT;
            CustomId = "custom-id";
            Options = Some [];
            ChannelTypes = None;
            Placeholder = None;
            DefaultValues = None;
            MinValues = None;
            MaxValues = None;
            Disabled = None;
        }

        let expected = """{"type":3,"custom_id":"custom-id","options":[],"channel_types":null,"placeholder":null,"default_values":null,"min_values":null,"max_values":null,"disabled":null}"""
        
        // Act
        let actual = FsJson.serialize selectMenu

        // Assert
        Assert.AreEqual<string>(expected, actual)
        
    [<TestMethod>]
    member _.ComponentConverter_Write_CorrectlySerializesTextInput () =
        // Arrange
        let textInput = Component.TextInput {
            Type = ComponentType.TEXT_INPUT;
            CustomId = "custom-id";
            Style = TextInputStyle.SHORT;
            Label = "Label";
            MinLength = None;
            MaxLength = None;
            Required = None;
            Value = None;
            Placeholder = None;
        }

        let expected = """{"type":4,"custom_id":"custom-id","style":1,"label":"Label","min_length":null,"max_length":null,"required":null,"value":null,"placeholder":null}"""

        // Act
        let actual = FsJson.serialize textInput

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    member _.InteractionCallbackDataConverter_Read_CorrectlyDeserializesAutocompleteData () =
        // Arrange
        let json = """{"choices":[]}"""

        let expected = InteractionCallbackData.Autocomplete {
            Choices = [];
        }

        // Act
        let actual = FsJson.deserialize<InteractionCallbackData> json

        // Assert
        Assert.AreEqual<InteractionCallbackData>(expected, actual)

    [<TestMethod>]
    member _.InteractionCallbackDataConverter_Read_CorrectlyDeserializesMessageData () =
        // Arrange
        let json = """{"tts":null,"content":"Content","embeds":null,"allowed_mentions":null,"flags":null,"components":null,"attachments":null,"poll":null}"""

        let expected = InteractionCallbackData.Message {
            Tts = None;
            Content = Some "Content";
            Embeds = None;
            AllowedMentions = None;
            Flags = None;
            Components = None;
            Attachments = None;
            Poll = None;
        }

        // Act
        let actual = FsJson.deserialize<InteractionCallbackData> json

        // Assert
        Assert.AreEqual<InteractionCallbackData>(expected, actual)

    [<TestMethod>]
    member _.InteractionCallbackDataConverter_Read_CorrectlyDeserializesModalData () =
        // Arrange
        let json = """{"custom_id":"custom-id","title":"Title","components":[]}"""

        let expected = InteractionCallbackData.Modal {
            CustomId = "custom-id";
            Title = "Title";
            Components = [];
        }

        // Act
        let actual = FsJson.deserialize<InteractionCallbackData> json

        // Assert
        Assert.AreEqual<InteractionCallbackData>(expected, actual)

    [<TestMethod>]
    member _.InteractionCallbackDataConverter_Read_FailsOnInvalidJsonString () =
        // Arrange
        let json = """{this is in valid json}"""

        // Act
        let res () = FsJson.deserialize<InteractionCallbackData> json |> ignore

        // Assert
        Assert.ThrowsException<JsonException> res |> ignore

    [<TestMethod>]
    member _.InteractionCallbackDataConverter_Write_CorrectlySerializesAutocompleteData () =
        // Arrange
        let autocomplete = InteractionCallbackData.Autocomplete {
            Choices = [];
        }

        let expected = """{"choices":[]}"""


        // Act
        let actual = FsJson.serialize autocomplete

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    member _.InteractionCallbackDataConverter_Write_CorrectlySerializesMessageData () =
        // Arrange
        let message = InteractionCallbackData.Message {
            Tts = None;
            Content = Some "Content";
            Embeds = None;
            AllowedMentions = None;
            Flags = None;
            Components = None;
            Attachments = None;
            Poll = None;
        }

        let expected = """{"tts":null,"content":"Content","embeds":null,"allowed_mentions":null,"flags":null,"components":null,"attachments":null,"poll":null}"""

        // Act
        let actual = FsJson.serialize message

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    member _.InteractionCallbackDataConverter_Write_CorrectlySerializesModalData () =
        // Arrange
        let modal = InteractionCallbackData.Modal {
            CustomId = "custom-id";
            Title = "Title";
            Components = [];
        }

        let expected = """{"custom_id":"custom-id","title":"Title","components":[]}"""

        // Act
        let actual = FsJson.serialize modal

        // Assert
        Assert.AreEqual<string>(expected, actual)
