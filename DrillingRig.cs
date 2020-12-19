


        //Var------------------------------------------------------------------------

        // Do you want the rotor and rig to spin clockwise or Counter Clockwise?
        //string rotorDirection = "clockwise"; //angle will be a positive value ie 90*
        string rotorDirection = "counterclockwise"; //angle will be a negitive value ie -45*


        List<IMyTerminalBlock> drillingRig;
        List<IMyShipDrill> rigDrills;
        List<IMyPistonBase> rigPistonsUp;
        List<IMyPistonBase> rigPistonsDown;
        List<IMyMotorAdvancedStator> rigRotor;
        List<IMyPistonBase> rigPistonsOut;
        List<IMyProgrammableBlock> rigProgrammableBlock;
        float rotorAngle;
        float pistonOutMaxLimit;
        float newrotorAngle;
        float incrementRotorby;
        string errorMessage;


        // Public Program ------------------------------------------------------------------
        public Program()
        {
            drillingRig = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlockGroupWithName("Drilling Rig").GetBlocks(drillingRig);
            rigDrills = new List<IMyShipDrill>();
            rigPistonsUp = new List<IMyPistonBase>();
            rigPistonsDown = new List<IMyPistonBase>();
            rigRotor = new List<IMyMotorAdvancedStator>();
            rigPistonsOut = new List<IMyPistonBase>();
            rigProgrammableBlock = new List<IMyProgrammableBlock>();



            foreach (var blk in drillingRig)
            {
                //Echo($"{blk.CustomName}");
                if (blk.CustomName == "Rig Drill")
                {
                    //Echo($"{blk.CustomName}");
                    rigDrills.Add(blk as IMyShipDrill);
                }
                else if (blk.CustomName == "Rig Piston Up")
                {
                    //Echo($"{blk.CustomName}");
                    rigPistonsUp.Add(blk as IMyPistonBase);
                }
                else if (blk.CustomName == "Rig Piston Down")
                {
                    //Echo($"{blk.CustomName}");
                    rigPistonsDown.Add(blk as IMyPistonBase);
                }
                else if (blk.CustomName == "Advanced Rotor")
                {
                    //Echo($"{blk.CustomName}");
                    rigRotor.Add(blk as IMyMotorAdvancedStator);
                }
                else if (blk.CustomName == "Rig Piston Out")
                {
                    //Echo($"{blk.CustomName}");
                    rigPistonsOut.Add(blk as IMyPistonBase);
                }
                else if (blk.CustomName == "Rig Programmable Block")
                {
                    //Echo($"{blk.CustomName}");
                    rigProgrammableBlock.Add(blk as IMyProgrammableBlock);
                };
            };

            rotorAngle = (float)(rigRotor[0].Angle * 180 / Math.PI);
            pistonOutMaxLimit = 0.0f;
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            incrementRotorby = 10; //the degree rotation of the rotor.  Always positive and works with counterclockwise rotation.
        }

        //Add error messages
        //Add ability to make more than one pass.  Probably just need pass counter for number of passes.  full circle would be 360 / 5.  watch for rotor for multipass 
        // add count of up and down pistons to set velocity.  ie velocity down = number of pistons / top speed 
        //one error is if someone moves the rotor.  the way to catch it is to compare actually rotor angle with rotorAngle var.  error is to recompile code to fix rotorAngle

        public void Main(string argument, UpdateType updateSource)
        {
            //Echo($"{errorMessage}");
            Echo($"Rotor angle is {rotorAngle}");
            Echo($"Next Rotor angle is {newrotorAngle}");
            //OnePass();
            //TestRotor();
            Test(355);

            //bug rotor going to unlimited
            //add if clockwise, update uper and lower limits
            //try it in new world starting with counterclockwise
            //rotor going clockwise instead of counterclockwise is fixed in test()

            //if at 5*, move upper to unlimited, move lower to 355 and it works fine.
            


        }

        //Main Logic -------------------------------------------------------------------------------------
        double stage = 10; //stage arbitrialy starts to 10.  Stage controlls drilling cycle
        void TestRotor ()
        {
            if (stage == 10)
            {
                UpdateRotorAngles();
                stage = 11;
            };
            if (stage == 11)
            {
                MoveRotor();
                stage = 12;
            };
            if (stage == 12)
            {
                rigProgrammableBlock[0].ApplyAction("OnOff_Off");
                stage = 10;
            };
        }
        void UpdateRotorAngles()
        {
            if (rotorDirection == "clockwise")
            {

            }
            if (rotorDirection == "counterclockwise")
            {
                newrotorAngle = rotorAngle - incrementRotorby;
            }

        }
            
        void MoveTheRotor(float incrementRotorby)
        {
            
            foreach (var rotor in rigRotor)
            {
                if (rotorDirection == "clockwise")
                {

                }
                if (rotorDirection == "counterclockwise")
                {
                    rotor.LowerLimitDeg = newrotorAngle;
                    rotor.UpperLimitDeg = newrotorAngle;
                    rotor.SetValue<float>("Velocity", -incrementRotorby);
                }
            } 

        }
            //---------------------------------------------------------------------------------
        
        void Test(float limitAngle)
        {
            
            foreach (var rotor in rigRotor)
            {
                rotor.LowerLimitDeg = limitAngle;
                rotor.UpperLimitDeg = (limitAngle + 10);
            }


        }

        void OnePass() //Drills a trench, a series of holes, then set the drill up for the next pass by rotating 5 degrees and turns the programmable block off
        {
            Echo($"It is on stage {stage}");
            if (stage == 10)
            {
                StartSetUp(11); //16 for current test,  11 normally
                TurnDrillsOn();                                              //reset velocity, set the uper and lower angles set up newrotorangle
            };
            if (stage == 11) //starts 1st hole
            {
                DownStroke();
                NextStage(12, 0);
            };
            if (stage == 12)
            {
                UpStroke();
                MoveFoward(3.3);
                NextStage(13, 10);
            };
            if (stage == 13) //starts 2nd hole
            {
                DownStroke();
                NextStage(14, 0);
            };
            if (stage == 14)
            {
                UpStroke();
                MoveFoward(6.6);
                NextStage(15, 10);
            };
            if (stage == 15) //starts 3rd hole
            {
                DownStroke();
                NextStage(16, 0);
            };
            if (stage == 16)
            {
                UpStroke();
                MoveFoward(10.0);
                NextStage(17, 10);
            };
            if (stage == 17) //starts 4th hole
            {
                DownStroke();
                NextStage(18, 0);                        
            };
            if (stage == 18)
            {
                UpStroke();                               //if at 5 *, move upper to unlimited and //if// drill is up moverotor, //increase one of the angles to newrotorangle,
                NextStage(19, 10);
            };
            if (stage == 19) //ends the drilling pass and sets up for next pass
            {
                StartSetUp(20); 
            };
            if (stage == 20) //may just need a nested if statment to repeat pass
            {
                TurnDrillsOff();                         //increase the other angle and set angle var to newrotorangle 
                NextStage(21, 10);
            };
            if (stage == 21)
            {
                
                rigProgrammableBlock[0].ApplyAction("OnOff_Off");    //reset velocity
                NextStage(10, 10);
            };
            if (stage == 2)
            {

            };
        }


        //low level functions
        void NextStage(float stagenumber, float rigPistonUpCurrentPosition) // to unreliable to use stage++, whould often skip steps.
        {
            if (rigPistonsUp[0].CurrentPosition == rigPistonUpCurrentPosition)
            {
                stage = stagenumber;
            }
        }
        void StartSetUp(float stagenumber) //Set up for 1st trench
        {
            foreach (var piston in rigPistonsUp)
            {
                piston.Velocity = 5f;
                piston.MaxLimit = 10;
                piston.MinLimit = 0;
            };
            foreach (var piston in rigPistonsDown)
            {
                piston.Velocity = -5f;
                piston.MaxLimit = 10;
                piston.MinLimit = 0;
            };
            foreach (var piston in rigPistonsOut)
            {
                piston.Velocity = -5f;
                piston.MaxLimit = 0;
                piston.MinLimit = 0;
            };

            if (rigPistonsUp[0].CurrentPosition == 10 & rigPistonsDown[0].CurrentPosition == 0 & rigPistonsOut[0].CurrentPosition == 0)
            {
                stage = stagenumber; 
            }
        }
        
        void UpdateNewRotorAngle()
        {
            if (rotorDirection == "clockwise")
            {
                if (rotorAngle > (360 - incrementRotorby)) //so 350
                {
                    rotorAngle -= 360;
                };
                newrotorAngle = rotorAngle + incrementRotorby;
            };
            if (rotorDirection == "counterclockwise")
            {
                if (rotorAngle < 0) //less than 10
                {
                    rotorAngle += 360;
                };
                newrotorAngle = rotorAngle - incrementRotorby;
            };
        }
        void DownStroke()
        {
            Echo("It's on a down Stroke");
            foreach (var piston in rigPistonsUp)
            {
                piston.Velocity = -0.1f;
            }
            foreach (var piston in rigPistonsDown)
            {
                piston.Velocity = 0.1f;
            }
        }
        void UpStroke()
        {
            Echo("It's on a up Stroke");
            foreach (var piston in rigPistonsUp)
            {
                piston.Velocity = 5f;
            }
            foreach (var piston in rigPistonsDown)
            {
                piston.Velocity = -5f;
            }

        }
        void MoveFoward(double pistonMaxLimit) //Move boon Forward 3.3m at a time.  this is what turns a hole into a trench
        {

            if (rigPistonsUp[0].CurrentPosition == 10)
            {
                foreach (var piston in rigPistonsOut)
                {
                    piston.Velocity = 5f;
                    piston.MaxLimit = (float)pistonMaxLimit;
                };
            }
        }
        void MoveRotor() //Moves the rotor inorderto set it up for the next pass by 5 degrees, left or right depending on var set on line 5
        {
            foreach (var rotor in rigRotor)
            {
                if (rotorDirection == "clockwise")
                {
                    rotor.LowerLimitDeg = newrotorAngle; 
                    rotor.UpperLimitDeg = newrotorAngle;
                    rotor.ApplyAction("IncreaseVelocity");
                };
                if (rotorDirection == "counterclockwise")
                {
                    rotor.LowerLimitDeg = newrotorAngle;
                    //rotor.UpperLimitDeg = newrotorAngle;  //causes rotor to spin clockwise
                    rotor.ApplyAction("DecreaseVelocity");
                };
            }
        }
        void TurnDrillsOff()
        {
            foreach (var drill in rigDrills)
            {
                drill.ApplyAction("OnOff_Off");
            }
        }
        void TurnDrillsOn()
        {
            foreach (var drill in rigDrills)
            {
                drill.ApplyAction("OnOff_On");
            }
        }


