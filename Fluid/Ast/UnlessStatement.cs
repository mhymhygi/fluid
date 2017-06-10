﻿using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace Fluid.Ast
{
    public class UnlessStatement : TagStatement
    {
        public UnlessStatement(
            Expression condition,
            IList<Statement> statements
            ) : base(statements)
        {
            Condition = condition;
        }

        public Expression Condition { get; }

        public override Completion WriteTo(TextWriter writer, TextEncoder encoder, TemplateContext context)
        {
            var result = Condition.Evaluate(context).ToBooleanValue();

            if (!result)
            {
                foreach (var statement in Statements)
                {
                    var completion = statement.WriteTo(writer, encoder, context);

                    if (completion != Completion.Normal)
                    {
                        // Stop processing the block statements
                        // We return the completion to flow it to the outer loop
                        return completion;
                    }
                }

                return Completion.Normal;
            }

            return Completion.Normal;
        }
    }
}
