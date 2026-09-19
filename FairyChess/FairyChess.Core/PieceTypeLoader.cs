using System.Text.Json;
using System.Text.Json.Serialization;

namespace FairyChess.Core
{
    /// <summary>
    /// Загрузка типов фигур из JSON (System.Text.Json).
    /// Ключи — camelCase-имена параметров: name, symbol, patterns (dx, dy, maxSteps,
    /// mirror, captureOnly, quietOnly), isRoyal, doubleFirstStep, promotesTo.
    /// Пропущенные ключи получают значения по умолчанию; неизвестные ключи игнорируются.
    /// </summary>
    public static class PieceTypeLoader
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            Converters = { new CharAsStringConverter() }
        };

        public static void Load(PieceTypeCatalog catalog, string json)
        {
            ArgumentNullException.ThrowIfNull(catalog);
            ArgumentException.ThrowIfNullOrWhiteSpace(json);

            List<PieceType>? types;
            try
            {
                types = JsonSerializer.Deserialize<List<PieceType>>(json, JsonOptions);
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Ошибка разбора JSON с типами фигур: {ex.Message}", ex);
            }

            if (types is null)
                throw new JsonException("Ожидался массив типов фигур, получено null.");

            // Два прохода: полная проверка до первой регистрации, чтобы не оставить каталог
            // в промежуточном состоянии при ошибке в середине списка.
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (var type in types)
            {
                type.Validate();
                if (!names.Add(type.Name))
                    throw new ArgumentException($"Дубликат типа фигуры '{type.Name}' в JSON.");
                if (catalog.Contains(type.Name))
                    throw new ArgumentException($"Тип фигуры '{type.Name}' уже зарегистрирован в каталоге.");
            }

            foreach (var type in types)
                catalog.Register(type);
        }

        public static void LoadFromFile(PieceTypeCatalog catalog, string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл с типами фигур не найден: '{path}'.", path);

            Load(catalog, File.ReadAllText(path));
        }

        /// <summary>char ↔ строка из одного символа: даёт детерминированное поведение на любой версии рантайма.</summary>
        private sealed class CharAsStringConverter : JsonConverter<char>
        {
            public override char Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string? s = reader.GetString();
                if (s is not { Length: 1 })
                    throw new JsonException($"Ожидался символ (строка из одного знака), получено: '{s}'.");

                return s[0];
            }

            public override void Write(Utf8JsonWriter writer, char value, JsonSerializerOptions options) =>
                writer.WriteStringValue(value.ToString());
        }
    }
}
