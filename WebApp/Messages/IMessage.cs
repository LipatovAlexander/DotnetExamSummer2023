﻿using System.Text.Json.Serialization;

namespace WebApp.Messages;

[JsonDerivedType(typeof(AddItem), "add_item")]
[JsonDerivedType(typeof(DeleteItem), "delete_item")]
[JsonDerivedType(typeof(EditItem), "edit_item")]
public interface IMessage {}