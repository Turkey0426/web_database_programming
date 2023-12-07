using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SimpleWordTest_comb_t : System.Web.UI.Page
{
    int count = 0;
    int corr = 0;
    int start;
    int end;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["count"] = 0;
            Session["corr"] = 0;
            Session["start"] = 1;
            Session["end"] = 10;

            if ((int)Session["start"] == 1)
            {
                CBF110007_PreviousButton.Enabled = false;
            }
            else if ((int)Session["end"] == GridView1.PageCount * 10)
            {
                CBF110007_NextButton.Enabled = false;
            }
            
        }


    }

    private void show()
    {
        count = (int)Session["count"];
        CBF110007_input.Text = "";
        CBF110007_ch_hint.Text = CBF110007_DDL2.Items[count].Value;
        for (int i = 0; i < CBF110007_DDL2.Items[count].Text.Length; i++)
        {
            if (i == 0)
            {
                CBF110007_input.Text += CBF110007_DDL2.Items[count].Text[0];
            }
            else
            {
                CBF110007_input.Text += "_";
            }
        }
    }

    private void check()
    {
        count = (int)Session["count"];
        if (CBF110007_input.Text == CBF110007_DDL2.Items[count].Text)
        {
            corr = (int)Session["corr"];
            corr++;
            Session["corr"] = corr;
            Response.Output.WriteLine("答對了<br />");
        }
        else
        {
            Response.Output.WriteLine($"答錯了！ 答案是{CBF110007_DDL2.Items[count].Text}<br />");
        }
    }

    protected void CBF110007_DDL1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CBF110007_cambridge.Text += $"<a href='https://dictionary.cambridge.org/zht/詞典/英語-漢語-繁體/{CBF110007_DDL1.SelectedItem}' target='_blank'>{CBF110007_DDL1.SelectedItem.ToString()}</a> =>  {CBF110007_DDL1.SelectedValue.ToString()}<br />";

    }

    protected void CBF110007_testBtn_Click(object sender, EventArgs e)
    {
        CBF110007_MV1.ActiveViewIndex = 1;
        show();
        CBF110007_input.Focus();
    }

    protected void CBF110007_DDL1_DataBound(object sender, EventArgs e)
    {
        Random r = new Random();
        List<ListItem> items = new List<ListItem>(CBF110007_DDL2.Items.Cast<ListItem>());

        for (int i = items.Count - 1; i > 0; i--)
        {
            int j = r.Next(i + 1);
            // 交換位置
            ListItem temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }

        CBF110007_DDL2.Items.Clear();
        CBF110007_DDL2.Items.AddRange(items.ToArray());

        for (int i = CBF110007_DDL2.Items.Count - 1; i > 9; i--)
        {
            CBF110007_DDL2.Items.Remove(CBF110007_DDL2.Items[i]);
        }

        CBF110007_cambridge.Text = $"<a href='https://dictionary.cambridge.org/zht/詞典/英語-漢語-繁體/{CBF110007_DDL1.SelectedItem}' target='_blank'>{CBF110007_DDL1.SelectedItem.ToString()}</a> =>  {CBF110007_DDL1.SelectedValue.ToString()}<br />";

    }

    protected void CBF110007_nextQBtn_Click(object sender, EventArgs e)
    {
        if (CBF110007_nextQBtn.Text == "結束")
        {
            Environment.Exit(0);
        }
        CBF110007_input.Focus();
        check();
        count = (int)Session["count"];
        count++;
        Session["count"] = count;
        if (count < 10)
        {
            show();
        }
        else
        {
            corr = (int)Session["corr"];
            Literal1.Text = $"總得分 : {corr * 10:F2}";
            CBF110007_nextQBtn.Text = "結束";
            CBF110007_ch_hint.Visible = false;
            CBF110007_input.Visible = false;
            HyperLink1.Visible = true;
        }
    }

    protected void CBF110007_PreviousButton_Click(object sender, EventArgs e)
    {
        CBF110007_NextButton.Enabled = true;
        start = (int)Session["start"];
        end = (int)Session["end"];
        start -= 10;
        end -= 10;
        Session["start"] = start;
        Session["end"] = end;
        SqlDataSource1.SelectCommand = $"SELECT * FROM [gept_words] WHERE ID BETWEEN {start} AND {end}";

        if ((int)Session["start"] == 1)
        {
            CBF110007_PreviousButton.Enabled = false;
        }
    }

    protected void CBF110007_NextButton_Click(object sender, EventArgs e)
    {
        CBF110007_PreviousButton.Enabled = true;
        start = (int)Session["start"];
        end = (int)Session["end"];
        start += 10;
        end += 10;
        Session["start"] = start;
        Session["end"] = end;
        SqlDataSource1.SelectCommand = $"SELECT * FROM [gept_words] WHERE ID BETWEEN {start} AND {end}";
        
        if ((int)Session["end"] == GridView1.PageCount*10)
        {
            CBF110007_NextButton.Enabled = false;
        }
    }
}
