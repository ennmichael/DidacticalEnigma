﻿using System;
using JDict.Internal.XmlModels;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Optional;
using Optional.Collections;

namespace JDict
{
    public class KanjiDict
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(KanjiDictRoot));

        private Dictionary<string, KanjiEntry> root;

        private KanjiDict Init(Stream stream)
        {
            var xmlSettings = new XmlReaderSettings
            {
                CloseInput = false,
                DtdProcessing = DtdProcessing.Parse,
                XmlResolver = null, // we don't want to resolve against external entities
                MaxCharactersFromEntities = 0,
                MaxCharactersInDocument = 32 * 1024 * 1024 / 2
            };
            using (var xmlReader = XmlReader.Create(stream, xmlSettings))
            {
                root = new Dictionary<string, KanjiEntry>();
                var entries = ((KanjiDictRoot)serializer.Deserialize(xmlReader)).Characters;
                foreach (var entry in entries)
                {
                    root.Add(entry.Literal, new KanjiEntry(entry));
                }
            }
            return this;
        }

        public Option<KanjiEntry> Lookup(string v)
        {
            return root.GetValueOrNone(v);
        }

        private KanjiDict Init(string path)
        {
            using (var file = File.OpenRead(path))
            using (var gzip = new GZipStream(file, CompressionMode.Decompress))
            {
                return Init(gzip);
            }
        }

        private KanjiDict()
        {

        }

        public static KanjiDict Create(string path)
        {
            return new KanjiDict().Init(path);
        }

        public static KanjiDict Create(Stream stream)
        {
            return new KanjiDict().Init(stream);
        }
    }

    public class KanjiEntry
    {
        public string Literal { get; }

        public int StrokeCount { get; }

        public int FrequencyRating { get; }

        internal KanjiEntry(KanjiCharacter ch)
        {
            Literal = ch.Literal;
            StrokeCount = ch.Misc.StrokeCount[0];
            FrequencyRating = ch.Misc.FrequencyRating != 0
                ? ch.Misc.FrequencyRating
                : 100000;
        }
    }
}