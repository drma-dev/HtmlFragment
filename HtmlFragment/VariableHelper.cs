using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlFragment
{
    public class VariableHelper
    {
        public const string style = "cursor: pointer; pointer-events: inherit; text-transform: inherit; font: inherit; color: inherit !important; text-decoration: underline; padding: 0; display: unset; vertical-align: unset; letter-spacing: normal;";

        public RenderFragment CreateGenericComponent(string text, object receiver, IMatDialogService MatDialogService) => builder =>
        {
            if (text == null) text = "";

            builder.OpenElement(0, "span");

            var sequence = 1;

            var variable_body = "{@button01}";

            var regex = new Regex(variable_body); //searches the text for this variable (may have more than one)

            var variables = regex.Matches(text);

            var before = text.Substring(0, variables[0].Index);
            builder.AddContent(sequence, before); sequence++;

            for (int i = 0; i < variables.Count; i++)
            {
                builder.AddContent(sequence, CreateLinkComponent(receiver, "button desc", MatDialogService)); sequence++;

                if (variables.Count > i + 1) //there is one more variable to process
                {
                    var curr_var = variables[i];
                    var next_var = variables[i + 1];

                    var between_var = text.Substring(curr_var.Index + curr_var.Length, next_var.Index - (curr_var.Index + curr_var.Length));
                    builder.AddContent(sequence, between_var); sequence++;
                }
            }

            var last_variable = variables[variables.Count - 1];
            var after = text.Substring(last_variable.Index + last_variable.Length, text.Length - (last_variable.Index + last_variable.Length));
            builder.AddContent(sequence, after); sequence++;

            if (sequence == 1)
            {
                builder.AddContent(sequence, text);
            }

            builder.CloseElement();
        };

        private RenderFragment CreateLinkComponent(object receiver, string value, IMatDialogService MatDialogService) => builder =>
        {
            if (string.IsNullOrEmpty(value)) return;

            builder.OpenComponent(0, typeof(MatButtonLink));
            builder.AddAttribute(1, "Style", style);
            builder.AddAttribute(2, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(receiver, () => RequestNewValue(MatDialogService)));
            builder.AddAttribute(3, "ChildContent", (RenderFragment)((builder2) =>
            {
                builder2.AddContent(4, value);
            }));
            builder.CloseComponent();
        };

        public async Task RequestNewValue(IMatDialogService MatDialogService)
        {
            await MatDialogService.AlertAsync("alert");

            //var att = new Dictionary<string, object>
            //{
            //    { "VariableName", Variable.Name },
            //    { "VariableValue", value }
            //};

            //var result = await dialog.OpenAsync(typeof(ChangeDialog), new MatDialogOptions() { Attributes = att });

            //if (result != null)
            //{
            //    await ChangeValue(repository, questionnaire, Variable, valueId, (string)result);
            //}
        }

        public async Task ChangeValue()
        {
            //if (valueId.HasValue)
            //{
            //    var obj = await repository.FindAsync<VariableValue>(valueId);

            //    if (obj != null)
            //    {
            //        obj.Value = newvalue;
            //        await repository.UpdateAsync(obj);
            //    }

            //    VariableUpdated.Invoke(obj);
            //}
            //else
            //{
            //    var obj = new VariableValue
            //    {
            //        Variable = Variable,
            //        Value = newvalue
            //    };

            //    questionnaire.Values.Add(obj);
            //    await repository.UpdateAsync(questionnaire);

            //    VariableUpdated.Invoke(obj);
            //}
        }
    }
}