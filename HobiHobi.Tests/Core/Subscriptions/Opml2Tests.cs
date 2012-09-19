using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HobiHobi.Tests.Core.Subscriptions
{
    public class Opml2Tests
    {

        string _sample = @"
onGetRiverStream ({
	""updatedFeeds"": {
		""updatedFeed"": [
			{
				""feedUrl"": ""http://static.scripting.com/pensacola/comments/rss.xml"",
				""websiteUrl"": ""http://threads2.scripting.com/"",
				""feedTitle"": ""Pioneering OPML comments"",
				""feedDescription"": ""These are the comments from the most innovative web people in the world, people who are leading the bootstrap into the post-Web 2.0 world."",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:27:05 GMT"",
				""item"": [
					{
						""body"": ""I believe I have the feature working, but I need a fresh comment to test it out. If this works, the river produced by Houston that includes this item will also have the JSON conversion of the OPML text you are reading right now. If it works, I expect my brain to explode! :-)"",
						""permaLink"": ""http://worknotes.scripting.com/september2012/91712ByDw/river2IsSourceurlaware#daveWiner"",
						""pubDate"": ""Mon, 17 Sep 2012 16:26:55 GMT"",
						""title"": ""Dave Winer comments on \""River2 is source-aware.\"""",
						""link"": ""http://worknotes.scripting.com/september2012/91712ByDw/river2IsSourceurlaware#daveWiner"",
						""source"": [
							{
	""opml"":
	{
		""version"": ""2.0"",
		""head"":
		{
			""title"": ""Comment from Dave Winer"",
			""dateCreated"": ""Mon, 17 Sep 2012 16:26:49 GMT"",
			""dateModified"": ""Mon, 17 Sep 2012 16:26:49 GMT"",
			""ownerName"": ""Dave Winer"",
			""ownerEmail"": ""dave.winer@gmail.com"",
			""expansionState"": """",
			""vertScrollState"": ""1"",
			""windowTop"": ""74"",
			""windowLeft"": ""1280"",
			""windowBottom"": ""563"",
			""windowRight"": ""2089""
			},
		""body"":
		{
			""outline"": [
				{
					""text"": ""I believe I have the feature working, but I need a fresh comment to test it out. "",
					""#value"": """"
					}
				,
				{
					""text"": ""If this works, the river produced by Houston that includes this item will also have the JSON conversion of the OPML text you are &lt;a href=\""http://scripting.com/images/2012/09/17/brainexplosion.gif\""&gt;reading&lt;/a&gt; right now. "",
					""#value"": """"
					}
				,
				{
					""text"": ""If it works, I expect my brain to explode! :-)"",
					""#value"": """"
					}
				
				]
			}
		}
	}

							],
						""id"": ""1640880""
						}
					]
				},
			{
				""feedUrl"": ""http://www.techmeme.com/index.xml"",
				""websiteUrl"": ""http://www.techmeme.com/"",
				""feedTitle"": ""Techmeme"",
				""feedDescription"": ""Tech Web, page A1"",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:56 GMT"",
				""item"": [
					{
						""body"": ""Ellis Hamburger / The Verge : Google takes on Instagram and Facebook by acquiring top iOS photo app Snapseed &nbsp; --&nbsp; Google has agreed to acquire Nik Software, the German developer of photography app Snapseed, for an undisclosed amount.&nbsp; Sources close to the deal..."",
						""permaLink"": ""http://www.techmeme.com/120917/p24#a120917p24"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:56 GMT"",
						""title"": ""Google takes on Instagram and Facebook by acquiring top iOS photo app Snapseed (Ellis Hamburger/The Verge)"",
						""link"": ""http://www.techmeme.com/120917/p24#a120917p24"",
						""id"": ""1640875""
						}
					,
					{
						""body"": ""Frederic Lardinois / TechCrunch : Duolingo Raises $15M Series B Round Led By NEA, Will Expand To More Languages And To Mobile Soon &nbsp; --&nbsp; Duolingo, the language learning and crowd-sourced translation service founded by reCAPTCHA founder and Carnegie Mellon professor..."",
						""permaLink"": ""http://www.techmeme.com/120917/p23#a120917p23"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:54 GMT"",
						""title"": ""Duolingo Raises $15M Series B Round Led By NEA, Will Expand To More Languages And To Mobile Soon (Frederic Lardinois/TechCrunch)"",
						""link"": ""http://www.techmeme.com/120917/p23#a120917p23"",
						""id"": ""1640872""
						}
					]
				},
			{
				""feedUrl"": ""http://www.theverge.com/rss/index.xml"",
				""websiteUrl"": ""http://www.theverge.com/"",
				""feedTitle"": ""The Verge -  All Posts"",
				""feedDescription"": """",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:53 GMT"",
				""item"": [
					{
						""body"": ""Just what does the Canon EOS 6D want to be? Reading the company's press release would suggest the answer's obvious &mdash; it's Canon's lightest, smallest and cheapest full-frame DSLR yet, squaring up against the similarly lauded D600 from Nikon &mdash; but viewed in the cold..."",
						""permaLink"": """",
						""pubDate"": ""Mon, 17 Sep 2012 15:22:45 GMT"",
						""title"": ""Canon's schizophrenic 6D doesn't know whether it's for pros or amateurs (video)"",
						""link"": ""http://www.theverge.com/2012/9/17/3346366/canon-6d-video-pictures-preview"",
						""id"": ""1640871""
						}
					,
					{
						""body"": ""Google has agreed to acquire Nik Software , the German developer of photography app Snapseed , for an undisclosed amount. Sources close to the deal tell The Verge that while Nik Software produces all sorts of apps for photographers like Color Efex Pro and Dfine for Mac and..."",
						""permaLink"": """",
						""pubDate"": ""Mon, 17 Sep 2012 15:31:31 GMT"",
						""title"": ""Google takes on Instagram and Facebook by acquiring top iOS photo app Snapseed"",
						""link"": ""http://www.theverge.com/2012/9/17/3346182/google-acquires-snapseed-nik-software"",
						""id"": ""1640868""
						}
					]
				},
			{
				""feedUrl"": ""http://www.techmeme.com/index.xml"",
				""websiteUrl"": ""http://www.techmeme.com/"",
				""feedTitle"": ""Techmeme"",
				""feedDescription"": ""Tech Web, page A1"",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:51 GMT"",
				""item"": [
					{
						""body"": ""Ben Worthen / Wall Street Journal : H-P Tries On a Sleeker Look &nbsp; --&nbsp; Taking a Cue From Apple, CEO Whitman Makes Better Design Central to Turnaround&nbsp; --&nbsp; When Meg Whitman took over as chief executive of Hewlett-Packard Co. a year ago, she got a company-issued..."",
						""permaLink"": ""http://www.techmeme.com/120917/p22#a120917p22"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:51 GMT"",
						""title"": ""H-P Tries On a Sleeker Look (Ben Worthen/Wall Street Journal)"",
						""link"": ""http://www.techmeme.com/120917/p22#a120917p22"",
						""id"": ""1640866""
						}
					]
				},
			{
				""feedUrl"": ""http://www.tedstake.com/feed/"",
				""websiteUrl"": """",
				""feedTitle"": ""Ted's Take"",
				""feedDescription"": """",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:49 GMT"",
				""item"": [
					{
						""body"": ""Who knew? &#160; Washington D.C. has more marketing and advertising professionals per capita than any other community in the country. &#160; For every 100,000 residents, there are 700 people employed as marketing and advertising executives. &#160; I try to keep &#8230; Continue reading &#8594; "",
						""permaLink"": """",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:49 GMT"",
						""title"": ""Great Stat for D.C. Business Community"",
						""link"": ""http://www.tedstake.com/2012/09/17/great-stat-for-d-c-business-community/"",
						""comments"": ""http://www.tedstake.com/2012/09/17/great-stat-for-d-c-business-community/#comments"",
						""id"": ""1640863""
						}
					]
				},
			{
				""feedUrl"": ""http://www.theverge.com/rss/index.xml"",
				""websiteUrl"": ""http://www.theverge.com/"",
				""feedTitle"": ""The Verge -  All Posts"",
				""feedDescription"": """",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:47 GMT"",
				""item"": [
					{
						""body"": ""When Samsung released the Galaxy Note , it was met with some skepticism. Do people want a huge phone with a huge screen that barely fits in your pocket, people wondered? Turns out they do, and the Note has sold gangbusters since it was introduced. It's been left mostly without..."",
						""permaLink"": """",
						""pubDate"": ""Mon, 17 Sep 2012 15:45:01 GMT"",
						""title"": ""LG Intuition review"",
						""link"": ""http://www.theverge.com/2012/9/17/3333108/lg-intuition-review"",
						""id"": ""1640861""
						}
					]
				},
			{
				""feedUrl"": ""http://www.techmeme.com/index.xml"",
				""websiteUrl"": ""http://www.techmeme.com/"",
				""feedTitle"": ""Techmeme"",
				""feedDescription"": ""Tech Web, page A1"",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:46 GMT"",
				""item"": [
					{
						""body"": ""Kim Zetter / Wired : Coders Behind the Flame Malware Left Incriminating Clues on Control Servers &nbsp; --&nbsp; The attackers behind the nation-state espionage tool known as Flame accidentally left behind tantalizing clues that provide information about their identity and that..."",
						""permaLink"": ""http://www.techmeme.com/120917/p27#a120917p27"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:46 GMT"",
						""title"": ""Coders Behind the Flame Malware Left Incriminating Clues on Control Servers (Kim Zetter/Wired)"",
						""link"": ""http://www.techmeme.com/120917/p27#a120917p27"",
						""id"": ""1640860""
						}
					]
				},
			{
				""feedUrl"": ""http://www.spiegel.de/international/index.rss"",
				""websiteUrl"": ""http://www.spiegel.de"",
				""feedTitle"": ""SPIEGEL ONLINE - International"",
				""feedDescription"": ""Daily news, analysis and opinion from Europe's leading newsmagazine and Germany's top news Web site."",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:45 GMT"",
				""item"": [
					{
						""body"": ""The German government is pleased with the recent decision by the country's Constitutional Court that gave the green light to ratify the permanent euro bailout fund. But the celebration may be premature. Some of the conditions set by the court could prove prickly for the..."",
						""permaLink"": ""http://www.spiegel.de/international/germany/unlimited-liability-legal-hurdles-ahead-in-effort-to-save-euro-a-856226.html"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:45 GMT"",
						""title"": ""Unlimited Liability: German Parties Offer&#160;Rival Interpretations of Euro Ruling"",
						""link"": ""http://www.spiegel.de/international/germany/unlimited-liability-legal-hurdles-ahead-in-effort-to-save-euro-a-856226.html#ref=rss"",
						""id"": ""1640858""
						}
					]
				},
			{
				""feedUrl"": ""http://www.theverge.com/rss/index.xml"",
				""websiteUrl"": ""http://www.theverge.com/"",
				""feedTitle"": ""The Verge -  All Posts"",
				""feedDescription"": """",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:44 GMT"",
				""item"": [
					{
						""body"": ""Microsoft has just started sending invites to members of the press for a Windows 8 launch in New York on October 25th. Microsoft previously announced that Windows 8 would go on sale on October 26th , but the launch event timing matches a previous rumor that Microsoft may be..."",
						""permaLink"": """",
						""pubDate"": ""Mon, 17 Sep 2012 15:53:19 GMT"",
						""title"": ""Windows 8 launch event to be held in New York on October 25th"",
						""link"": ""http://www.theverge.com/2012/9/17/3347088/windows-8-launch-new-york-october-25th"",
						""id"": ""1640857""
						}
					]
				},
			{
				""feedUrl"": ""http://www.techmeme.com/index.xml"",
				""websiteUrl"": ""http://www.techmeme.com/"",
				""feedTitle"": ""Techmeme"",
				""feedDescription"": ""Tech Web, page A1"",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:40 GMT"",
				""item"": [
					{
						""body"": ""Emil Protalinski / The Next Web : As predicted, Google+ passes 400M registered users, now has 100M monthly active users &nbsp; --&nbsp; As part of Google's announcement to acquire Snapseed's creator Nik software, Vic Gundotra, the Senior Vice President of Engineering for Google,..."",
						""permaLink"": ""http://www.techmeme.com/120917/p26#a120917p26"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:40 GMT"",
						""title"": ""As predicted, Google+ passes 400M registered users, now has 100M monthly active users (Emil Protalinski/The Next Web)"",
						""link"": ""http://www.techmeme.com/120917/p26#a120917p26"",
						""id"": ""1640854""
						}
					]
				},
			{
				""feedUrl"": ""http://www.spiegel.de/international/index.rss"",
				""websiteUrl"": ""http://www.spiegel.de"",
				""feedTitle"": ""SPIEGEL ONLINE - International"",
				""feedDescription"": ""Daily news, analysis and opinion from Europe's leading newsmagazine and Germany's top news Web site."",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:40 GMT"",
				""item"": [
					{
						""body"": ""One of Germany's most unusual aquatic sports competitions features dozens of paddlers in non-conventional vessels. Vying for gourd glory, they splash their way to the finish line in enormous pumpkins. The Ludwigsburg Palace regatta has become one of the country's most photogenic..."",
						""permaLink"": ""http://www.spiegel.de/international/zeitgeist/german-city-hosts-floating-pumpkin-race-a-856273.html"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:40 GMT"",
						""title"": ""Pumpkins Ahoy!: Germany's Drenched Veggie Regatta"",
						""link"": ""http://www.spiegel.de/international/zeitgeist/german-city-hosts-floating-pumpkin-race-a-856273.html#ref=rss"",
						""id"": ""1640853""
						}
					]
				},
			{
				""feedUrl"": ""http://www.theverge.com/rss/index.xml"",
				""websiteUrl"": ""http://www.theverge.com/"",
				""feedTitle"": ""The Verge -  All Posts"",
				""feedDescription"": """",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:40 GMT"",
				""item"": [
					{
						""body"": ""I stepped out of my car in the parking lot of Google&rsquo;s Mountain View headquarters and, not having a clue which way to go, started following the man ambling down the sidewalk wearing a very odd backpack. A giant spherical camera stuck up behind his shoulders, bouncing above..."",
						""permaLink"": """",
						""pubDate"": ""Mon, 17 Sep 2012 16:09:27 GMT"",
						""title"": ""Failure is a feature: how Google stays sharp gobbling up startups"",
						""link"": ""http://www.theverge.com/2012/9/17/3322854/google-startup-mergers-acquisitions-failure-is-a-feature"",
						""id"": ""1640852""
						}
					]
				},
			{
				""feedUrl"": ""http://www.thedailybeast.com/content/dailybeast/feed/david-frum.rss.xml"",
				""websiteUrl"": ""http://www.thedailybeast.com/feed/david-frum.rss.xml"",
				""feedTitle"": ""david-frum"",
				""feedDescription"": ""david-frum"",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:37 GMT"",
				""item"": [
					{
						""body"": ""In my column for CNN, I urge President Obama to proudly and publicly defend free speech , even (and especially) when it is of the unpopular variety. "",
						""permaLink"": ""http://www.thedailybeast.com/articles/2012/09/17/memo-to-obama-speak-for-free-speech.html"",
						""pubDate"": ""Mon, 17 Sep 2012 16:00:00 GMT"",
						""title"": ""Memo to Obama: Speak for Free Speech"",
						""link"": ""http://www.thedailybeast.com/articles/2012/09/17/memo-to-obama-speak-for-free-speech.html"",
						""id"": ""1640850""
						}
					]
				},
			{
				""feedUrl"": ""http://www.techmeme.com/index.xml"",
				""websiteUrl"": ""http://www.techmeme.com/"",
				""feedTitle"": ""Techmeme"",
				""feedDescription"": ""Tech Web, page A1"",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:36 GMT"",
				""item"": [
					{
						""body"": ""Mary Jo Foley / ZDNet : Microsoft confirms October 25 launch for Windows 8 &nbsp; --&nbsp; Summary: Microsoft officials are now confirming the Windows 8 -- and Surface RT -- launch will be on October 25 in New York City.&nbsp; --&nbsp; Mary Jo Foley&nbsp; --&nbsp; It's not a big..."",
						""permaLink"": ""http://www.techmeme.com/120917/p25#a120917p25"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:36 GMT"",
						""title"": ""Microsoft confirms October 25 launch for Windows 8 (Mary Jo Foley/ZDNet)"",
						""link"": ""http://www.techmeme.com/120917/p25#a120917p25"",
						""id"": ""1640849""
						}
					]
				},
			{
				""feedUrl"": ""http://www.spiegel.de/international/index.rss"",
				""websiteUrl"": ""http://www.spiegel.de"",
				""feedTitle"": ""SPIEGEL ONLINE - International"",
				""feedDescription"": ""Daily news, analysis and opinion from Europe's leading newsmagazine and Germany's top news Web site."",
				""whenLastUpdate"": ""Mon, 17 Sep 2012 16:20:35 GMT"",
				""item"": [
					{
						""body"": """",
						""permaLink"": ""http://www.spiegel.de/international/picture-this-in-top-form-a-856364.html"",
						""pubDate"": ""Mon, 17 Sep 2012 16:20:35 GMT"",
						""title"": ""Picture This: Fasten Your Seatbelts"",
						""link"": ""http://www.spiegel.de/international/picture-this-in-top-form-a-856364.html#ref=rss"",
						""id"": ""1640847""
						}
						]
					}
				]
			},
		""metadata"": {
			""docs"": ""http://scripting.com/stories/2010/12/06/innovationRiverOfNewsInJso.html"",
			""whenGMT"": ""Mon, 17 Sep 2012 16:28:01 GMT"",
			""whenLocal"": ""9/17/2012; 12:28:01 PM Eastern"",
			""version"": ""3"",
			""secs"": ""0.383""
			}
		}
)";
    }
}
