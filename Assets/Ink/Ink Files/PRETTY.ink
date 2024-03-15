EXTERNAL SetSpeaker(character)
VAR soloStartDialogue = ""
VAR soloMainDialogue = ""
VAR soloEndDialogue = ""

VAR prettyEmbrodyleStartDialogue = ""
VAR prettyEmbrodyleMainDialogue = ""

VAR prettyPearceStartDialogue = ""
VAR prettyPearceMainDialogue = ""


//Every character pair should have a StartDialogue and MainDialogue (EX: prettyEmbrodyleStartDialogue or prettyEmbrodyleMainDialogue)
VAR multiStartDialogue = ""
VAR multiMainDialogue = ""

//Set for each character's special function
VAR functionDialogue = ""

//Speaker variable passed in from unity that determines which pair is speaking. If it's just this one character, it'll be set to SOLO
// I'm going to set it up in unity so that each pair is just the two names side by side in the same order as it is in the document. (EX: PRETTYEMBRODYLE, and prettyPearce)
//Ms. pretty is also abbreviated to PRETTY because of weird capitalization 
VAR speakers = ""

VAR soloDialogueChoice = 2
~soloDialogueChoice = "{1|2|3|4|5}"

VAR multiDialogueChoice = 1
~multiDialogueChoice = "{1|2|3}"


VAR prettyEmbrodyleDialogueChoice = 1
~prettyEmbrodyleDialogueChoice = "{1|2|3|4|5}"


VAR prettyPearceDialogueChoice = 1
~prettyPearceDialogueChoice = "{1|2|3|}"


//SOLO
~soloStartDialogue = "{~Hello, darling kitty! I’m so happy to see you!|How are you, kitty?|Oh, what fun!|My, how lucky to see you!}"
~soloMainDialogue = ""
~soloEndDialogue = "{~Are you leaving so soon? My, you’re in a hurry.|I wish you the best, darling kitty!|Come see me again soon!.}"
//SOLO


~functionDialogue = "{~blah blah blah| yap yap yap}"


//prettyEmbrodyleMULTI
~prettyEmbrodyleStartDialogue = "{~Hello, darling kitty! I was just having a nice chat with Embrodyle.|Haha! You charmer, you. You’ll make a little lady blush!|Of course I have time for you, kitty!|Kitty, my darling! No, I’m not busy at all. What is it?}"
~prettyEmbrodyleMainDialogue = ""

//prettyPearceMULTI
~prettyPearceStartDialogue = "{My! Come look kitty! You’re just in time for a new trick!|Stupendous Pearce, always such a lovely time with you.|Oh, kitty! I’ve missed you and your darling whiskers.|My, Pearce is such a lovely fellow but he can’t replace you and your dashing little bow.}"
~prettyPearceMainDialogue = ""
{
- speakers:
    {speakers == "SOLO": -> SOLO}
    {speakers == "FUNCTION": -> FUNCTION}
    {speakers == PRETTYEMBRODYLE: -> PRETTYEMBRODYLE}
    {speakers == PRETTYPEARCE: -> PRETTYPEARCE}
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
}


==SOLOCHOICE1==
Quite the little soldier aren't you, kitty!
I’ve quite the admiration for your adventurous spirit, such a lovely little thing you are.
Oh my, off again? Do come pay a visit, I’ll miss your charming little whiskers!
{soloEndDialogue}
->END

==SOLOCHOICE2==
Oh my, I do hope you’re staying safe out there, kitty!
It can be quite dangerous, I couldn’t bear to lose another darling treasure.
Oh, my! Not that anything could happen to you, it most certainly would not.
{soloEndDialogue}
->END

==SOLOCHOICE3==
You’re such a dashing little kitty!
Have you noticed we match? Well, maybe only a little! 
I’m certain you’re far cuter than me, I just know it. 
No, no, no arguing! Don’t you know no one can say no to me? Quite the tradition it is in the family! 
{soloEndDialogue}
->END

==SOLOCHOICE4==
Do you have a family, darling kitty?
My, you don’t know? I would have hoped a charming bunch of littermates like you were scampering about the world. 
Do I?
My, I wonder! What a joy that might be. 
{soloEndDialogue}
->END

==SOLOCHOICE5==
Ohh…! that shopkeeper is quite a mean one!
His yelling is far too startling for me, how do you put up with it kitty?
Perhaps quite a frightening one too… 
{soloEndDialogue}
->END



==FUNCTION==
//If the character doesn't have random function lines then you can just put those here 
{functionDialogue}
->END

==PRETTYEMBRODYLE==
//same structure as solo except with only two blocks since there's no end dialogue for multi convos
{prettyEmbrodyleMainDialogue}
{
- prettyEmbrodyleDialogueChoice:
    {prettyEmbrodyleDialogueChoice == 1: -> PRETTYEMBRODYLEMULTICHOICE1}
    {prettyEmbrodyleDialogueChoice == 2: -> PRETTYEMBRODYLEMULTICHOICE2}
    {prettyEmbrodyleDialogueChoice == 3: -> PRETTYEMBRODYLEMULTICHOICE3}
    {prettyEmbrodyleDialogueChoice == 4: -> PRETTYEMBRODYLEMULTICHOICE4}
    {prettyEmbrodyleDialogueChoice == 5: -> PRETTYEMBRODYLEMULTICHOICE5}
}
-> END


==PRETTYEMBRODYLEMULTICHOICE1==
 Have you seen Embrodyle’s feathers, darling kitty?
He’s quite the stubborn little one, but I’m certain I can clean him up! I’m rather stubborn too, you know?
If you’d like, I can give your fur a brush as well.

-> END

==PRETTYEMBRODYLEMULTICHOICE2==
 Embrodyle is such a gentleman, isn’t he? 
I heard that he helps you out from time to time. If I may ask, what is it he does? 
He collects items you lose? 
So, he has the bravery to travel back to that horrid, dangerous place. How dashing! 

-> END

==PRETTYEMBRODYLEMULTICHOICE3==
I’m awfully jealous of how Embrodyle takes such good care of his appearance. 
I know I should do the same, but it’s so hard when I’m such a busy lady! 
Hm? Busy doing what, you ask? 
Busy cheering you on, of course! 

-> END

==PRETTYEMBRODYLEMULTICHOICE4==
My, Embrodyle is quite the reserved owl.
Have you noticed as well, kitty?
It’s quite the mystery! I’m so curious!

-> END


==PRETTYEMBRODYLEMULTICHOICE5==
~SetSpeaker("Pretty")
Embrodyle, do you ever feel something may be missing?
~SetSpeaker("Embrodyle")
I don’t believe I quite understand what you mean.
~SetSpeaker("Pretty")
Oh, I’m just being a little silly aren’t I? Not something to worry about.
~SetSpeaker("Embrodyle")
Do not be so quick to dissuade yourself from your feelings, Ms. Pretty. It would be wise to listen to your thoughts. 
~SetSpeaker("Pretty")
But what if they are wrong?
~SetSpeaker("Embrodyle")
Then you must fight to not be led astray. 
~SetSpeaker("Pretty")
hank you, Embrodyle… 

-> END
==PRETTYPEARCE==
//same structure as solo except with only two blocks since there's no end dialogue for multi convos
{prettyPearceMainDialogue}
{
- prettyEmbrodyleDialogueChoice:
    {prettyPearceDialogueChoice == 1: -> PRETTYPEARCEMULTICHOICE1}
    {prettyPearceDialogueChoice == 2: -> PRETTYPEARCEMULTICHOICE2}
    {prettyPearceDialogueChoice == 3: -> PRETTYPEARCEMULTICHOICE3}

}
-> END

==PRETTYPEARCEMULTICHOICE1==
Darling kitty, may I let you in on a secret? 
The truth is, Pearce’s magic tricks aren’t all too impressive. 
But it makes him so happy when I act excited, so I can’t help myself from doing so. 
Don’t tell him, alright? It’ll be just between you and me.

-> END

==PRETTYPEARCEMULTICHOICE2==
I wonder what keeps Pearce so joyful all the time! 
Perhaps I could ask him his darling secret.
Or perhaps he has nothing to miss? 
Oh! How silly of me, that was rather sudden wasn’t it, kitty? 

-> END

==PRETTYPEARCEMULTICHOICE3==
~SetSpeaker("Pretty")
Oh, how wonderful Pearce! Your shows always take my breath away!
~SetSpeaker("Pearce")
Aha! As I knew it would. I have experience beyond the ages.
~SetSpeaker("Pretty")
My, Pearce, would you mind teaching me a few of your tricks?
~SetSpeaker("Pearce")
Oh- I- yes! Of course, that would be magnifique! I’ve always wanted to have an apprentice.
~SetSpeaker("Pretty")
Well, you’re quite dazzling! I’d be ever so grateful to learn!
~SetSpeaker("Pearce")
Then learn, you shall!  

-> END

