EXTERNAL SetSpeaker(character)
VAR soloStartDialogue = ""
VAR soloMainDialogue = ""
VAR soloEndDialogue = ""

VAR embrodylePrettyStartDialogue = ""
VAR embrodylePrettyMainDialogue = ""

VAR embrodylePearceStartDialogue = ""
VAR embrodylePearceMainDialogue = ""


//Every character pair should have a StartDialogue and MainDialogue (EX: embrodylePrettyStartDialogue or embrodylePrettyMainDialogue)
VAR multiStartDialogue = ""
VAR multiMainDialogue = ""

//Set for each character's special function
VAR functionDialogue = ""

//Speaker variable passed in from unity that determines which pair is speaking. If it's just this one character, it'll be set to SOLO
// I'm going to set it up in unity so that each pair is just the two names side by side in the same order as it is in the document. (EX: EMBRODYLEPRETTY, and embrodylePearce)
//Ms. pretty is also abbreviated to PRETTY because of weird capitalization 
VAR speakers = ""

VAR soloDialogueChoice = 2
~soloDialogueChoice = "{1|2|3|4|5}"

VAR multiDialogueChoice = 1
~multiDialogueChoice = "{1|2|3}"


VAR embrodylePrettyDialogueChoice = 1
~embrodylePrettyDialogueChoice = "{1|2|3|4|5}"


VAR embrodylePearceDialogueChoice = 1
~embrodylePearceDialogueChoice = "{1|2|3|}"


//SOLO
~soloStartDialogue = "{Greetings, cat. I hope things are finding you well.|Have you need of me?|Pray tell, what do you have need of this time?}"
~soloMainDialogue = ""
~soloEndDialogue = "{Farewell, cat. Do not get yourself in any unnecessary trouble. |Very well. |Good day. |Pleasant evening.}"

//SOLO


~functionDialogue = "{~blah blah blah| yap yap yap}"


//embrodylePrettyMULTI
~embrodylePrettyStartDialogue = "{~Hello, cat. I am simply chatting with Ms. Pretty. |You make for lovely company, Ms. Pretty.|Cat. Don’t fret, I can always spare a moment for you.}"
~embrodylePrettyMainDialogue = ""

//embrodylePearceMULTI
~embrodylePearceStartDialogue = "{~Pearce is going on about his magic nonsense again. Please, do you have anything to ask of me? |I cannot stand to hear another ounce of this fool’s rambling any longer.|…Cat. How are you?}"
~embrodylePearceMainDialogue = ""
{
- speakers:
    {speakers == "SOLO": -> SOLO}
    {speakers == "FUNCTION": -> FUNCTION}
    {speakers == EMBRODYLEPRETTY: -> EMBRODYLEPRETTY}
    {speakers == EMBRODYLEPEARCE: -> EMBRODYLEPEARCE}
}

==SOLO==
{soloStartDialogue}
{
- soloDialogueChoice:
    {soloDialogueChoice == 1: -> SOLOCHOICE1}
    {soloDialogueChoice == 2: -> SOLOCHOICE2}
    {soloDialogueChoice == 3: -> SOLOCHOICE3}
    {soloDialogueChoice == 4: -> SOLOCHOICE4}
    {soloDialogueChoice == 5: -> SOLOCHOICE5}
    {soloDialogueChoice == 5: -> SOLOCHOICE6}
}


==SOLOCHOICE1==
I feel something is off with that shopkeeper. I have never seen his shoddy little stall before. 
Furthermore, he refuses to sell anything to me. Not that I’m looking to acquire any of his cheap wares anyway. 
…Perhaps I’m making unfair judgment on a stranger. Please forget what I have just told you. 

{soloEndDialogue}
->END

==SOLOCHOICE2==
You’re curious about my feathers? 
Please do not touch them at the moment. 
This isn’t a slight against you in any way, cat. They have just become terribly dirty due to being trapped in that filthy cage. 
You may touch them when I allow you to. Do take care not to pull on them. 

{soloEndDialogue}
->END

==SOLOCHOICE3==
How are you, cat? 
… 
It appears you’re even shorter of words than me. I’m impressed. 
Just where did you come from? I don’t believe I’ve seen you before. 
You don’t know…? Curious. 

{soloEndDialogue}
->END

==SOLOCHOICE4==
You’re rather small aren’t you?
I could easily snap you up in my talons. 
Though, I wouldn’t want to dirty them so quickly. 
It’s quite a headache to clean my feathers from fresh blood.

{soloEndDialogue}
->END

==SOLOCHOICE5==
Do not look up at me like that, cat.
I might have some rather unpleasant thoughts.
A joke? 
Ah, yes, of course it was. 
As you know, I’m quite the comedian am I not?

{soloEndDialogue}
->END

==SOLOCHOICE6==
Have you ever been worshiped, cat?
…
No? Good. Best you keep it that way. 

{soloEndDialogue}
->END


==FUNCTION==
//If the character doesn't have random function lines then you can just put those here 
{functionDialogue}
->END

==EMBRODYLEPRETTY==
//same structure as solo except with only two blocks since there's no end dialogue for multi convos
{embrodylePrettyMainDialogue}
{
- embrodylePrettyDialogueChoice:
    {embrodylePrettyDialogueChoice == 1: -> EMBRODYLEPRETTYMULTICHOICE1}
    {embrodylePrettyDialogueChoice == 2: -> EMBRODYLEPRETTYMULTICHOICE2}
    {embrodylePrettyDialogueChoice == 3: -> EMBRODYLEPRETTYMULTICHOICE3}
    {embrodylePrettyDialogueChoice == 4: -> EMBRODYLEPRETTYMULTICHOICE4}
}
-> END


==EMBRODYLEPRETTYMULTICHOICE1==
I truly must assure Ms. Pretty I have no need of more company. 
More than two is far too much of a crowd. 
I tend to lose a fragment of my composure in such a place.
I do not believe you’d like to see that, cat. It’s rather undignified.  

-> END

==EMBRODYLEPRETTYMULTICHOICE2==
Perhaps I’m growing rather fond of Ms. Pretty, or perhaps I’m simply a bit too comfortable.
After all, one shan’t remain without some hesitations. It’s rather… uncouth. 
Remember that well, cat. 
-> END

==EMBRODYLEPRETTYMULTICHOICE3==
I must admit, despite my reservations, I find a few conversations here and there rather welcome. 
Ms. Pretty is an excellent change of pace.
From what, you ask, cat?
From things that ought not be remembered. 
-> END

==EMBRODYLEPRETTYMULTICHOICE4==
~SetSpeaker("Embrodyle")
Do you fear death, Ms Pretty?
~SetSpeaker("Pretty")
Oh! What a question; I don’t think about it too much. What good may come out of that?
~SetSpeaker("Embrodyle")
Wouldn’t you say it’s rather… important to be aware of your mortality. All things do die, after all.
~SetSpeaker("Pretty")
Oh, Embrodyle, that is why we enjoy the time we have while we have it! What use is thinking about all those big things when I can instead spend it talking to you!
~SetSpeaker("Embrodyle")
Mm… you have quite the perspective. One I am unused to.
 do hope that is a good thing!
~SetSpeaker("Embrodyle")
We shall see… 

-> END

==EMBRODYLEPEARCE==
//same structure as solo except with only two blocks since there's no end dialogue for multi convos
{embrodylePearceMainDialogue}
{
- embrodylePrettyDialogueChoice:
    {embrodylePearceDialogueChoice == 1: -> EMBRODYLEPEARCEMULTICHOICE1}
    {embrodylePearceDialogueChoice == 2: -> EMBRODYLEPEARCEMULTICHOICE2}
    {embrodylePearceDialogueChoice == 3: -> EMBRODYLEPEARCEMULTICHOICE3}

}
-> END

==EMBRODYLEPEARCEMULTICHOICE1==
I’m here to entertain Pearce’s delusions for a bit, nothing more. 
Did you know? One out of every five things he says has a ten percent chance of being useful in one of four situations. 
Everything else makes for decent white noise. 

-> END

==EMBRODYLEPEARCEMULTICHOICE2==
If you may, would you take my spot in this conversation? 
No? Fair enough. I suppose you are the busiest one here. 
It is just… 
I fear that I may experience mental deterioration if I am to stay here with him. 

-> END

==EMBRODYLEPEARCEMULTICHOICE3==
~SetSpeaker("Embrodyle")
Enough of this nonsense, Pearce, we all have far better things to do than watch this gimmick.
~SetSpeaker("Pretty")
Gimmick?? Embrodyle, you are simply what one might call a hater!
~SetSpeaker("Embrodyle")
Your words are wilting to my ears, foolish magician.
~SetSpeaker("Pretty")
Then allow me to liven your spirits with my NEXT TRICK!
~SetSpeaker("Embrodyle")
…Perhaps I may un-aliven your spirits if you subject me to more of these foolhardy games.


-> END

