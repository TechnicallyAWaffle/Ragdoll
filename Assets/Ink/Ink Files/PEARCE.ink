EXTERNAL SetSpeaker(character)
VAR soloStartDialogue = ""
VAR soloMainDialogue = ""
VAR soloEndDialogue = ""

VAR pearceEmbrodyleStartDialogue = ""
VAR pearceEmbrodyleMainDialogue = ""

VAR pearcePrettyStartDialogue = ""
VAR pearcePrettyMainDialogue = ""


//Every character pair should have a StartDialogue and MainDialogue (EX: pearceEmbrodyleStartDialogue or pearceEmbrodyleMainDialogue)
VAR multiStartDialogue = ""
VAR multiMainDialogue = ""

//Set for each character's special function
VAR functionDialogue = ""

//Speaker variable passed in from unity that determines which pair is speaking. If it's just this one character, it'll be set to SOLO
// I'm going to set it up in unity so that each pair is just the two names side by side in the same order as it is in the document. (EX: PEARCEEMBRODYLE, and pearcePretty)
//Ms. pretty is also abbreviated to PRETTY because of weird capitalization 
VAR speakers = ""

VAR soloDialogueChoice = 2
~soloDialogueChoice = "{1|2|3}"

VAR multiDialogueChoice = 1
~multiDialogueChoice = "{1|2|3}"


VAR pearceEmbrodyleDialogueChoice = 1
~pearceEmbrodyleDialogueChoice = "{1|2|3}"


VAR pearcePrettyDialogueChoice = 1
~pearcePrettyDialogueChoice = "{1|2}"


//SOLO
~soloStartDialogue = "{~How may I help you, my asphyxiated feline friend?|You came to see me? Magnifique!|Have you come to see a trick of mine?}"
~soloMainDialogue = ""
~soloEndDialogue = "{~May fate bring us again soon!|Let me know if my magic may be of use!|For this next trick, I shall now… disappear!}"
//SOLO


~functionDialogue = "{~blah blah blah| yap yap yap}"


//pearceEmbrodyleMULTI
~pearceEmbrodyleStartDialogue = "{~You’re just in time, my feline friend! I’m about to show Sir Embrodyle my newest trick!| Well, Sir Embrodyle? Was that not impressive? I can see the smile on your face! | Feline! How good of you to visit me! I was just chatting with Sir Embrodyle.}"

~pearceEmbrodyleMainDialogue = ""

//pearcePrettyMULTI
~pearcePrettyStartDialogue = "{Aha! Feline, Ms. Pretty is the loveliest audience I’ve had in years, allow me to give you a little taste of what I’ve shown her.|I believe I need a volunteer for this next one! Feline? Or perhaps you, Ms Pretty?}"
~pearcePrettyMainDialogue = ""
{
- speakers:
    {speakers == "SOLO": -> SOLO}
    {speakers == "FUNCTION": -> FUNCTION}
    {speakers == PEARCEEMBRODYLE: -> PEARCEEMBRODYLE}
    {speakers == PEARCEPRETTY: -> PEARCEPRETTY}
}

==SOLO==
{soloStartDialogue}
{
- soloDialogueChoice:
    {soloDialogueChoice == 1: -> SOLOCHOICE1}
    {soloDialogueChoice == 2: -> SOLOCHOICE2}
    {soloDialogueChoice == 3: -> SOLOCHOICE3}

}


==SOLOCHOICE1==
What is it? Have you finally decided to learn magic from me, feline? 
Oh, wondrous day! Where shall we start? The basic hat tricks? Disappearing tricks? 
…You want to try sawing yourself in half? 
Feline… Isn’t that cheating? 
{soloEndDialogue}
->END

==SOLOCHOICE2==
I could not tell you how glad I am to be out of that cage! 
Talent like this cannot be contained! It must be unleashed upon the world for everyone to see! 
Pearce, the magician who brings a smile to your face and a tear to your eye… 
…is finally able to work his magic again! 
{soloEndDialogue}
->END

==SOLOCHOICE3==
Are you on good terms with that shopkeeper? 
I tried performing a few tricks for him, but he brushed me off every time! 
One time, he even yelled at me and I dropped all my cards on the ground in a panic… 
No matter! A magician never gives up! I swear I shall find a trick that makes him burst with glee! 
{soloEndDialogue}
->END




==FUNCTION==
//If the character doesn't have random function lines then you can just put those here 
{functionDialogue}
->END

==PEARCEEMBRODYLE==
//same structure as solo except with only two blocks since there's no end dialogue for multi convos
{pearceEmbrodyleMainDialogue}
{
- pearceEmbrodyleDialogueChoice:
    {pearceEmbrodyleDialogueChoice == 1: -> PEARCEEMBRODYLEMULTICHOICE1}
    {pearceEmbrodyleDialogueChoice == 2: -> PEARCEEMBRODYLEMULTICHOICE2}
    {pearceEmbrodyleDialogueChoice == 3: -> PEARCEEMBRODYLEMULTICHOICE3}

}
-> END


==PEARCEEMBRODYLEMULTICHOICE1==
Sir Embrodyle seems unimpressed by my tricks. But no matter! This means I simply haven’t found the right one yet! 
Right, this means that I’ll keep performing them until one makes him smile! 
And if I run out, I’ll simply come up with more! My plan is foolproof! 
-> END

==PEARCEEMBRODYLEMULTICHOICE2==
I tried performing a trick where I pretended to steal Sir Embrodyle’s beak and make it vanish. 
To put it lightly, he was not 
happy with that. 
Doesn’t the man have a sense of humor? I don’t think it was deserving of a threat with his claws! 
-> END

==PEARCEEMBRODYLEMULTICHOICE3==
~SetSpeaker("Pearce")
Pearce: Sir Embrodyle! Please, watch closely. 
Pearce: (performs a trick)
Pearce: Magnifique! Don’t you think so too? 
~SetSpeaker("Embrodyle")
Embrodyle: I’m more amused with how enthusiastic you are about this outdated entertainment. 
~SetSpeaker("Pearce")
Pearce: Outdated?! Magic is a classic art form! It spreads cheer and whimsy! 
Pearce: Just look at your smile and tell me that it doesn’t work!
~SetSpeaker("Embrodyle")
Embrodyle: …
~SetSpeaker("Pearce")
Pearce: … 
~SetSpeaker("Embrodyle")
Embrodyle: …… 
~SetSpeaker("Pearce")
Pearce: Ahem! Now, for my next trick… 
 
-> END

==PEARCEPRETTY==
//same structure as solo except with only two blocks since there's no end dialogue for multi convos
{pearcePrettyMainDialogue}
{
- pearceEmbrodyleDialogueChoice:
    {pearcePrettyDialogueChoice == 1: -> PEARCEPRETTYMULTICHOICE1}
    {pearcePrettyDialogueChoice == 2: -> PEARCEPRETTYMULTICHOICE2}

}
-> END

==PEARCEPRETTYMULTICHOICE1==
Ah! Feline! It’s getting rather busy here, isn’t it?
I must say, though, having a growing audience is exhilarating!
Almost like coming home after a long trip away!
I must prepare more tricks, a new show. You best come when I’m prepared, feline!


-> END

==PEARCEPRETTYMULTICHOICE2==
~SetSpeaker("Pearce")
And for my next trick, I would like a volunteer. If you may, Ms Pretty?
~SetSpeaker("Pretty")
Oh, joy! I would be honored, Pearce.
~SetSpeaker("Pearce")
My hands are here! My hands are there! And now there is a flower behind your eyelash! 
~SetSpeaker("Pretty")
Oh, how lovely! Where could that have possibly come from?
~SetSpeaker("Pearce")
A magician never reveals their tricks, Ms Pretty. You may keep the flower, however!  

-> END

