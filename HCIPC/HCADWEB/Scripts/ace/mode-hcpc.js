/*
 * Código original licenciado sob a BSD baseado no trabalho pertencente a Ajax.org B.V., no Modo hcpc do Projeto ACE em https://ace.c9.io/
 * Modificações feitas no código são licenciadas seguindo a licença do projeto geral.
 */
/* ***** BEGIN LICENSE BLOCK *****
 * Distributed under the BSD license:
 *
 * Copyright (c) 2012, Ajax.org B.V.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of Ajax.org B.V. nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL AJAX.ORG B.V. BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * ***** END LICENSE BLOCK ***** */

ace.define("ace/mode/hcpc_highlight_rules", ["require", "exports", "module", "ace/lib/oop", "ace/mode/text_highlight_rules"], function (require, exports, module) {
    "use strict";

    var oop = require("../lib/oop");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

    var hcpcHighlightRules = function () {
        var keywordMapper = this.createKeywordMapper({
            "keyword.control": "algoritmo|fimalgoritmo|escreva|escreval|leia|se|enquanto|repita|fimse|fimenquanto|ate|até|mod|função|funcao|procedimento|fimfuncao|fimfunção|fimprocedimento|faca|faça|entao|então|senão|senao|e|ou|inicio|var|inteiro|real|logico|lógico|verdadeiro|falso"
        }, "identifier", true);

        this.$rules = {
            start: [{
                caseInsensitive: true,
                token: ['variable', "text",
                    'storage.type.prototype',
                    'entity.name.function.prototype'
                ],
                regex: '\\b(funcao|função|procedimento)(\\s+)(\\w+)(\\.\\w+)?(?=(?:\\(.*?\\))?;\\s*(?:externo))'
            }, {
                caseInsensitive: true,
                token: ['variable', "text", 'storage.type.function', 'entity.name.function'],
                regex: '\\b(funcao|função|procedimento)(\\s+)(\\w+)(\\.\\w+)?'
            }, {
                caseInsensitive: true,
                token: keywordMapper,
                regex: /\b[a-z_]+\b/
            }, {
                token: 'constant.numeric',
                regex: '\\b((0(x|X)[0-9a-fA-F]*)|(([0-9]+\\.?[0-9]*)|(\\.[0-9]+))((e|E)(\\+|-)?[0-9]+)?)(L|l|UL|ul|u|U|F|f|ll|LL|ull|ULL)?\\b'
            }, {
                token: 'punctuation.definition.comment',
                regex: '--.*$'
            }, {
                token: 'punctuation.definition.comment',
                regex: '//.*$'
            }, {
                token: 'punctuation.definition.comment',
                regex: '\\(\\*',
                push: [{
                    token: 'punctuation.definition.comment',
                    regex: '\\*\\)',
                    next: 'pop'
                },
                { defaultToken: 'comment.block.one' }
                ]
            }, {
                token: 'punctuation.definition.comment',
                regex: '\\{',
                push: [{
                    token: 'punctuation.definition.comment',
                    regex: '\\}',
                    next: 'pop'
                },
                { defaultToken: 'comment.block.two' }
                ]
            }, {
                token: 'punctuation.definition.string.begin',
                regex: '"',
                push: [{ token: 'constant.character.escape', regex: '\\\\.' },
                {
                    token: 'punctuation.definition.string.end',
                    regex: '"',
                    next: 'pop'
                },
                { defaultToken: 'string.quoted.double' }
                ]
            }, {
                token: 'punctuation.definition.string.begin',
                regex: '\'',
                push: [{
                    token: 'constant.character.escape.apostrophe',
                    regex: '\'\''
                },
                {
                    token: 'punctuation.definition.string.end',
                    regex: '\'',
                    next: 'pop'
                },
                { defaultToken: 'string.quoted.single' }
                ]
            }, {
                token: 'keyword.operator',
                regex: '[+\\-;,/*%]|:=|=|<\\-'
            }
            ]
        };

        this.normalizeRules();
    };

    oop.inherits(hcpcHighlightRules, TextHighlightRules);

    exports.hcpcHighlightRules = hcpcHighlightRules;
});

            ace.define("ace/mode/folding/coffee", ["require", "exports", "module", "ace/lib/oop", "ace/mode/folding/fold_mode", "ace/range"], function (require, exports, module) {
                "use strict";

                var oop = require("../../lib/oop");
                var BaseFoldMode = require("./fold_mode").FoldMode;
                var Range = require("../../range").Range;

                var FoldMode = exports.FoldMode = function () { };
                oop.inherits(FoldMode, BaseFoldMode);

                (function () {

                    this.getFoldWidgetRange = function (session, foldStyle, row) {
                        var range = this.indentationBlock(session, row);
                        if (range)
                            return range;

                        var re = /\S/;
                        var line = session.getLine(row);
                        var startLevel = line.search(re);
                        if (startLevel == -1 || line[startLevel] != "#")
                            return;

                        var startColumn = line.length;
                        var maxRow = session.getLength();
                        var startRow = row;
                        var endRow = row;

                        while (++row < maxRow) {
                            line = session.getLine(row);
                            var level = line.search(re);

                            if (level == -1)
                                continue;

                            if (line[level] != "#")
                                break;

                            endRow = row;
                        }

                        if (endRow > startRow) {
                            var endColumn = session.getLine(endRow).length;
                            return new Range(startRow, startColumn, endRow, endColumn);
                        }
                    };
                    this.getFoldWidget = function (session, foldStyle, row) {
                        var line = session.getLine(row);
                        var indent = line.search(/\S/);
                        var next = session.getLine(row + 1);
                        var prev = session.getLine(row - 1);
                        var prevIndent = prev.search(/\S/);
                        var nextIndent = next.search(/\S/);

                        if (indent == -1) {
                            session.foldWidgets[row - 1] = prevIndent != -1 && prevIndent < nextIndent ? "inicio" : "";
                            return "";
                        }
                        if (prevIndent == -1) {
                            if (indent == nextIndent && line[indent] == "#" && next[indent] == "#") {
                                session.foldWidgets[row - 1] = "";
                                session.foldWidgets[row + 1] = "";
                                return "inicio";
                            }
                        } else if (prevIndent == indent && line[indent] == "#" && prev[indent] == "#") {
                            if (session.getLine(row - 2).search(/\S/) == -1) {
                                session.foldWidgets[row - 1] = "inicio";
                                session.foldWidgets[row + 1] = "";
                                return "";
                            }
                        }

                        if (prevIndent != -1 && prevIndent < indent)
                            session.foldWidgets[row - 1] = "inicio";
                        else
                            session.foldWidgets[row - 1] = "";

                        if (indent < nextIndent)
                            return "inicio";
                        else
                            return "";
                    };

                }).call(FoldMode.prototype);

            });

            ace.define("ace/mode/hcpc", ["require", "exports", "module", "ace/lib/oop", "ace/mode/text", "ace/mode/hcpc_highlight_rules", "ace/mode/folding/coffee"], function (require, exports, module) {
                "use strict";

                var oop = require("../lib/oop");
                var TextMode = require("./text").Mode;
                var hcpcHighlightRules = require("./hcpc_highlight_rules").hcpcHighlightRules;
                var FoldMode = require("./folding/coffee").FoldMode;

                var Mode = function () {
                    this.HighlightRules = hcpcHighlightRules;
                    this.foldingRules = new FoldMode();
                    this.$behaviour = this.$defaultBehaviour;
                };
                oop.inherits(Mode, TextMode);

                (function () {

                    this.lineCommentStart = ["--", "//"];
                    this.blockComment = [
                        { start: "(*", end: "*)" },
                        { start: "{", end: "}" }
                    ];

                    this.$id = "ace/mode/hcpc";
                }).call(Mode.prototype);

                exports.Mode = Mode;
            });                (function () {
                ace.require(["ace/mode/hcpc"], function (m) {
                    if (typeof module == "object" && typeof exports == "object" && module) {
                        module.exports = m;
                    }
                });
            })();
