using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _FishDataMgr : SingletonManager<_FishDataMgr>
{
    public bool casualMode = false;

    private List<int> qualifiedFishes = new List<int>();
    private List<float> posibilities = new List<float>();

    public int currentBait = 0; 

    //等待时长控制在1.5~7之间
    //pd,fd都是半径
    public _FishData[] fishDatas = new _FishData[10000];

    public void InitItemsInfo()
    {
        for (int i = 0; i < 10000; ++i)
        {
            _FishData item = new _FishData();
            item.fishID = -1;
            item.num = 0;
            item.totalNum = 0;

            fishDatas[i] = item;
        }
        fishDatas[0] = new _FishData
        {
            fishID = 000,
            fishName = "Soul Fragments",
            fishDiscription = "The secret recipe from The Cat.\nWhen looking at it, you feel like you are looking at yourself.",
            mapIDs = new int[1] { -2 },
            baitIDs = new int[1] { -2 },
            beginWaitTime = 6.6f,
            endWaitTime = 7f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, false, false, false, false, false, false, false, false, false },
            possibility = 0.0f,
            strength = 0,
            scale = 0.0f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 50f,
            restIndex = 0.7f,
            notContainindex = 0.5f,
            num = 999999,
            totalNum =999999,
            spaceStormIndex =1
        };
        fishDatas[001] = new _FishData
        {
            fishID = 001,
            fishName = "Spaceweed",
            fishDiscription = "[Extracted from the ship's built-in database]\n\nAn air-borne creature. It converts the radiation of the star into energy, which it uses to sustain life. Each leaf is actually an individual head growing from its stem. Such head dies a few solar minutes after separation. Yes, this is an animal rather than a plant. ",
            mapIDs = new int[1] { 0 },
            baitIDs = new int[1] { 0 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, false, false, true, false, false, false, true, false, false },
            possibility = 0.6f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 50f,
            restIndex = 0.7f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.5f
        };
        fishDatas[002] = new _FishData
        {
            fishID = 002,
            fishName = "Jellyfish",
            fishDiscription = "[Extracted from the ship's built-in database]\n\nA romantic symbol in the eyes of humans. Named after a similar creature that once co-existed with humans on their mother planet, Earth. Unlike its Earthean counterpart, this species is air-borne.",
            mapIDs = new int[1] { 0 },
            baitIDs = new int[2] { 0,1 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 5.0f,
            beginHookTime = 2.5f,
            rushMid = new bool[10] { false, false, true, false, true, false, false, false, true, false },
            possibility = 0.45f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 42f,
            restIndex = 0.7f,
            notContainindex = 0.3f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 2f
        };
        fishDatas[003] = new _FishData
        {
            fishID = 003,
            fishName = "Ghostfish",
            fishDiscription = "[Extracted from the previous researcher's notes]\n\nShining beautifully under rays of light, the Ghostfish inhabits this desserted sector. Perhaps it is not accurate to say that this sector is desserted - we are just the first intelligent species to explore here. Back to this lovely creature. Despite its name, it is actually very meek. When frightened, its entire face (the darker part with two eyes) retreats into its shell. ",
            mapIDs = new int[1] { 0 },
            baitIDs = new int[2] { 0,1 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, true, false, false, true, false, true, false, true, false },
            possibility = 0.3f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 50f,
            restIndex = 0.7f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 1f
        };
        fishDatas[004] = new _FishData
        {
            fishID = 004,
            fishName = "Spaceponge",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nPlentiful within the Newland Colony as well as the Human's mother planet, Spaceponges grow in vacuum, as well as environments with generally thin 'air'. Similar to what one might think, the 'limbs' on top are actually eyes. ",
            mapIDs = new int[1] { 0 },
            baitIDs = new int[3] { 2,3,5 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { true, false, false, false, false, false, false, true, false, false },
            possibility = 0.5f,
            strength = 2,
            scale = 0.2f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 50f,
            restIndex = 0.7f,
            notContainindex = 0.55f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.5f
        };
        fishDatas[005] = new _FishData
        {
            fishID = 005,
            fishName = "Lost Spaceship",
            fishDiscription = "[My shiplog - Entry 450] \n\nThis little spaceship was one of the Human's. It was a model from the early space era, now an economical solution for casual traveling, popular among civilian families. Such a pity that the people in it were all dead now. It's been a long time I talked to a human. Doing research for the Cat is a lonely job after all. ",
            mapIDs = new int[1] { 0 },
            baitIDs = new int[2] { 2,3 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 8.0f,
            beginHookTime = 3.5f,
            rushMid = new bool[10] { false, true, false, false, true, false, true, false, false, false },
            possibility = 0.5f,
            strength = 2,
            scale = 0.2f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 55f,
            restIndex = 0.6f,
            notContainindex = 0.6f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 1f
        };
        fishDatas[006] = new _FishData
        {
            fishID = 006,
            fishName = "Flatfish",
            fishDiscription = "[My shiplog - Entry 511] \n\nThis funny looking fish was found in the mercury ocean underneath, as if its head was chopped off. But it was actually born this way. Primitive and unintelligent as it was, it was not scared of me nor the Cat. Intelligent creatures act very differently. They are normally scared of me, as well as this flat-faced thing. ",
            mapIDs = new int[1] { 0 },
            baitIDs = new int[2] { 4, 5 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 9f,
            beginHookTime = 3f,
            rushMid = new bool[10] { false, false, false, false, true, false, true, false, false, false },
            possibility = 0.6f,
            strength = 3,
            scale = 0.25f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 55f,
            restIndex = 0.7f,
            notContainindex = 0.6f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 1f
        };
        fishDatas[101] = new _FishData
        {
            fishID = 101,
            fishName = "Fly Crawler Drone",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nA failed redesign of the Fly Grabber Drone. It does not work well in the harsh environment of the Spaceship Graveyard, and is quickly abandoned by the AI military.",
            mapIDs = new int[1] { 1 },
            baitIDs = new int[1] { 0 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, false, false, false, false, false, false, false, false, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 53f,
            restIndex = 0.7f,
            notContainindex = 0.7f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[102] = new _FishData
        {
            fishID = 102,
            fishName = "Fly Grabber Drone",
            fishDiscription = "[Extracted from the previous researcher's notes] \n\nThis reminds me of flies on Earth, with wings and little legs...not that I've seen any real ones. I've actually never been to Earth myself! I'm happy here, doing my job, serving my Gods.",
            mapIDs = new int[1] { 1 },
            baitIDs = new int[3] { 0,101,103 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 9.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, false, true, false, true, false, false, false, true, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 50f,
            restIndex = 0.7f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[103] = new _FishData
        {
            fishID = 103,
            fishName = "Floater Resistance Ship",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nHuman resistance force owns this spaceship. It was designed with human skulls in mind, aiming to bring its opponents dreadful feelings.",
            mapIDs = new int[1] { 1 },
            baitIDs = new int[3] { 0,101,102 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 9.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, true, false, false, true, false, true, false, true, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 50f,
            restIndex = 0.7f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[104] = new _FishData
        {
            fishID = 104,
            fishName = "Lander Resistance Ship",
            fishDiscription = "[My shiplog - Entry 477] \n\nI caught another Human resistance force spaceship! This one looks a bit different that the floating one - it has tentacles protruding from the bottom. They definitely wanted it to look scary. Did it ever cross their minds that the AIs did not have skulls nor human bodies, therefore to them the ship was not scary at all?",
            mapIDs = new int[1] { 1 },
            baitIDs = new int[3] { 102,103,105 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 8.0f,
            beginHookTime = 1.5f,
            rushMid = new bool[10] { true, false, false, false, false, false, false, true, false, false },
            possibility = 0.7f,
            strength = 2,
            scale = 0.2f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 53f,
            restIndex = 0.7f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[105] = new _FishData
        {
            fishID = 105,
            fishName = "Jelly Grabber Drone",
            fishDiscription = "[Extracted from the previous researcher's notes] \n\nUsed by the AIs all around the Spaceship Graveyard to retrieve data samples. Early AI engineers surely got their ideas from Eartheans.",
            mapIDs = new int[1] { 1 },
            baitIDs = new int[3] {102,103,104 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 9.0f,
            beginHookTime = 2.5f,
            rushMid = new bool[10] { false, true, false, false, true, false, true, false, false, false },
            possibility = 0.7f,
            strength = 2,
            scale = 0.2f,
            pD = 0.5f,
            fD = 0.3f,
            fSpeed = 58f,
            restIndex = 0.7f,
            notContainindex = 0.7f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[106] = new _FishData
        {
            fishID = 106,
            fishName = "Plasma Officer",
            fishDiscription = "[Extracted from the previous researcher's notes] \n\nI rarely caught any members from the AI's side. This seems to be a medium ranked AI military officer! If I were not a researcher serving my God, but a civilian in the Empire of the Earthen Sapiens, I would punch this guy in the face! Assume it has a face for me to punch on. ",
            mapIDs = new int[1] { 1 },
            baitIDs = new int[2] { 104,105 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 9.0f,
            beginHookTime = 3.5f,
            rushMid = new bool[10] { false, false, true, false, true, false, true, false, false, false },
            possibility = 1.4f,
            strength = 3,
            scale = 0.25f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 68f,
            restIndex = 0.6f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[201] = new _FishData
        {
            fishID = 201,
            fishName = "Spaceweed_X",
            fishDiscription = "[Extracted from the previous researcher's notes] \n\nDid you know that this thing is totally edible (and even fun to eat)?? Most of the Humans enjoy it! I should probably introduce it to my God one day, though they don't really need to eat anything, except soul energy...",
            mapIDs = new int[1] { 2 },
            baitIDs = new int[3] { 0,202,203 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, false, false, true, false, false, false, true, false, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 50f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[202] = new _FishData
        {
            fishID = 202,
            fishName = "Jellyfish_X",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nThe variant of Jellyfish. Generations after generations being exposed to the system's radiation, this species gains an ability to attract object as a means of attack. ",
            mapIDs = new int[1] { 2 },
            baitIDs = new int[3] { 0,201,203 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, false, false, true, false, false, false, true, false, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.2f,
            fSpeed = 50f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[203] = new _FishData
        {
            fishID = 203,
            fishName = "Ghostfish_X",
            fishDiscription = "[Extracted from the previous researcher's notes] \n\nThe variant of Ghostfish that is unique to my home system. They can attract objects to attack their enemies.",
            mapIDs = new int[1] { 2 },
            baitIDs = new int[3] { 0,201,202 },
            beginWaitTime = 2.5f,
            endWaitTime = 4f,
            successHookTime = 8.0f,
            beginHookTime = 2.5f,
            rushMid = new bool[10] { false, true, false, false, true, false, true, false, true, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.1f,
            fSpeed = 60f,
            notContainindex = 0.6f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.7f
        };
        fishDatas[204] = new _FishData
        {
            fishID = 204,
            fishName = "Shuttle",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nA scout ship under the military of the Empire of Earthen Sapiens' command. Its primary duty is to patrol the region of the empire, if any danger occurs, report to the Department of Defense. In case of extreme danger, the Head of the Department of Defense has the responsibility to report to the Pope Himself.",
            mapIDs = new int[1] { 2 },
            baitIDs = new int[3] { 201,202,203 },
            beginWaitTime = 1.5f,
            endWaitTime = 6f,
            successHookTime = 8.5f,
            beginHookTime = 4.0f,
            rushMid = new bool[10] { true, false, false, false, false, false, false, true, false, false },
            possibility = 1.5f,
            strength = 2,
            scale = 0.2f,
            pD = 0.5f,
            fD = 0.2f,
            fSpeed = 58f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.7f
        };
        fishDatas[205] = new _FishData
        {
            fishID = 205,
            fishName = "Holy Scout Ship",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nA scout ship under the military of the Empire of Earthen Sapiens' command. Its primary duty is to patrol the region of the empire, if any danger occurs, report to the Department of Defense. In case of extreme danger, the Head of the Department of Defense has the responsibility to report to the Pope Himself.",
            mapIDs = new int[1] { 2 },
            baitIDs = new int[1] { 204 },
            beginWaitTime = 4f,
            endWaitTime = 5f,
            successHookTime = 8.0f,
            beginHookTime = 3.5f,
            rushMid = new bool[10] { false, false, false, false, true, false, true, false, false, false },
            possibility = 0.5f,
            strength = 2,
            scale = 0.18f,
            pD = 0.5f,
            fD = 0.3f,
            fSpeed = 58f,
            notContainindex = 0.6f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.5f
        };
        fishDatas[206] = new _FishData
        {
            fishID = 206,
            fishName = "Holy Flagship",
            fishDiscription = "[My shiplog - Entry 510] \n\nIt seemed that the Pope's direct army came to investigate. This flagship did have some strong weapons, and it took me some effort to finally take it down. Those people fought so fiercefully, I wondered, because of their faith in their God, or because of the fear for ours?",
            mapIDs = new int[1] { 2 },
            baitIDs = new int[2] { 204,205 },
            beginWaitTime = 1.5f,
            endWaitTime = 6f,
            successHookTime = 8.5f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { true, false, false, false, false, false, false, true, false, false },
            possibility = 0.1f,
            strength = 3,
            scale = 0.18f,
            pD = 0.5f,
            fD = 0.2f,
            fSpeed = 60f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.7f
        };
        fishDatas[301] = new _FishData
        {
            fishID = 301,
            fishName = "Egg",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nAn egg (Oo'arr, meaning 'egg') of the Swarm. If put under a temperature around -70 Celsius, it hatches the fastest. It is also noteworthy that the 'thing' trapped inside is an organic being's body, which provides nutrients for the youngling bug, once its soul is formed.",
            mapIDs = new int[1] { 3 },
            baitIDs = new int[1] { 0 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, false, false, true, false, false, false, true, false, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.15f,
            fSpeed = 50f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[302] = new _FishData
        {
            fishID = 302,
            fishName = "Larva",
            fishDiscription = "[Extracted from the previous researcher's notes] \n\nWe Humans know so little about the Swarm, or the 'Brotherhood', whatever you call them, except that they are a great hivemind, and a highly developed society. ",
            mapIDs = new int[1] { 3 },
            baitIDs = new int[3] { 0, 301, 303 },
            beginWaitTime = 2.5f,
            endWaitTime = 5.5f,
            successHookTime = 7.0f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { false, false, false, true, false, false, false, true, false, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.2f,
            fSpeed = 50f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 4.5f
        };
        fishDatas[303] = new _FishData
        {
            fishID = 303,
            fishName = "Teen",
            fishDiscription = "[Extracted from the previous researcher's notes] \n\nThis thing looks weird. Only a portion of adult bugs are designed to become soldiers, according to their highly stratified society structure, and this one will definitely not become a soldier.",
            mapIDs = new int[1] { 3 },
            baitIDs = new int[3] { 0, 301, 302 },
            beginWaitTime = 2.5f,
            endWaitTime = 4f,
            successHookTime = 8.0f,
            beginHookTime = 2.5f,
            rushMid = new bool[10] { false, true, false, false, true, false, true, false, true, false },
            possibility = 0.5f,
            strength = 1,
            scale = 0.1f,
            pD = 0.5f,
            fD = 0.1f,
            fSpeed = 60f,
            notContainindex = 0.6f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.7f
        };
        fishDatas[304] = new _FishData
        {
            fishID = 304,
            fishName = "Bro",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nA standard unit of the Swarm. Each and every one of them considered a 'brother', a swarm army of The Brotherhood could be viewed as a huge, closely related family. Can fight both individually and as a hivemind. ",
            mapIDs = new int[1] { 3 },
            baitIDs = new int[3] { 301, 302, 303 },
            beginWaitTime = 1.5f,
            endWaitTime = 6f,
            successHookTime = 8.5f,
            beginHookTime = 4.0f,
            rushMid = new bool[10] { true, false, false, false, false, false, false, true, false, false },
            possibility = 1.5f,
            strength = 2,
            scale = 0.2f,
            pD = 0.5f,
            fD = 0.2f,
            fSpeed = 58f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.7f
        };
        fishDatas[305] = new _FishData
        {
            fishID = 305,
            fishName = "A Caring Brother",
            fishDiscription = "[My shiplog - Entry 553] \n\nI believed this bug approached my ship because some of the younger members of its kind were caught. Despite its abhorrent features, it was in fact a caring and loving individual. I asked the Cat if It could revive the dea d bugs, but it did not care enough. ",
            mapIDs = new int[1] { 3 },
            baitIDs = new int[1] { 304 },
            beginWaitTime = 4f,
            endWaitTime = 5f,
            successHookTime = 8.0f,
            beginHookTime = 3.5f,
            rushMid = new bool[10] { false, false, false, false, true, false, true, false, false, false },
            possibility = 0.5f,
            strength = 2,
            scale = 0.18f,
            pD = 0.5f,
            fD = 0.3f,
            fSpeed = 58f,
            notContainindex = 0.6f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.5f
        };
        fishDatas[306] = new _FishData
        {
            fishID = 306,
            fishName = "Bro Cluster",
            fishDiscription = "[Extracted from the ship's built-in database] \n\nCombining their standard combat units, three (or even more) individual soldiers form a small cluster that work and fight as one single mind. Their consciousness is shared via a telepathic link.  ",
            mapIDs = new int[1] { 3 },
            baitIDs = new int[2] { 304, 305 },
            beginWaitTime = 1.5f,
            endWaitTime = 6f,
            successHookTime = 8.5f,
            beginHookTime = 3.0f,
            rushMid = new bool[10] { true, false, false, false, false, false, false, true, false, false },
            possibility = 0.1f,
            strength = 3,
            scale = 0.18f,
            pD = 0.5f,
            fD = 0.2f,
            fSpeed = 60f,
            notContainindex = 0.5f,
            num = 0,
            totalNum = 0,
            spaceStormIndex = 0.7f
        };

    }

    public int[] GetBaitAttraction(int _fishID)
    {
        if (fishDatas[_fishID].fishID < 0) return null;

        List<int> array = new List<int>();
        for (int i = 0; i < fishDatas.Length; i++)
        {
            if (fishDatas[i].fishID <= 0) continue;
            for(int j=0; j< fishDatas[i].baitIDs.Length; j++)
            {
                if (fishDatas[i].baitIDs[j] == _fishID)
                {
                    array.Add(i);
                    continue;
                }
            }
        }

        return array.ToArray();
    }

    public string GetBaitAttractionString(int _fishID)
    {
        string s = "{ ";
        int[] array = GetBaitAttraction(_fishID);
        for(int i = 0; i < array.Length; i++)
        {
            if (i != 0) s += ", ";
            s += array[i].ToString("D3");
        }
        s += " }";
        return s;
    }


    public _FishData GetFishByID(int _fishID)
    {

        if (fishDatas[_fishID].fishID == _fishID) return fishDatas[_fishID];

        return null;
    }

    public _FishData NewFish()
    {
        qualifiedFishes.Clear();
        posibilities.Clear();

        for (int i=0;i<fishDatas.Length;i++)
        {
            if (fishDatas[i].fishID <= 0) continue;
            bool inMap = false;
            for(int j = 0; j < fishDatas[i].mapIDs.Length; j++)
            {
                if (fishDatas[i].mapIDs[j] == MapMgr.GetInstance().GetMapByInt())
                {
                    inMap = true;
                    break;
                }
            }
            if (!inMap) continue;

            bool rightBait = false;
            for(int j = 0; j < fishDatas[i].baitIDs.Length; j++)
            {
                if (fishDatas[i].baitIDs[j] == currentBait)
                {
                    rightBait = true;
                    break;
                }
            }
            if (!rightBait) continue;

            qualifiedFishes.Add(i);
            posibilities.Add(fishDatas[i].possibility);
        }
        if (qualifiedFishes.Count == 0)
        {
            return fishDatas[0];
        }
        else
        {
            float sum = 0;
            foreach (float posibility in posibilities)
            {
                sum += posibility;
            }

            sum = Random.Range(0.0f, sum);

            int fishCount = 0;
            for (fishCount = 0; fishCount < posibilities.Count; fishCount++)
            {
                sum -= posibilities[fishCount];
                if (sum <= 0) break;
            }

            return fishDatas[qualifiedFishes[fishCount]];
        }
        
    }
}
