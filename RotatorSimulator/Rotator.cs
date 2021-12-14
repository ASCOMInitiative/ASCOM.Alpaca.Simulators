//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Rotator driver for RotatorSimulator
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Rotator interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Rotator Driver template
// --------------------------------------------------------------------------------
//
using System.Runtime.InteropServices;
using System.Collections;
using ASCOM;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Alpaca;
using System;
using System.Collections.Generic;
using ASCOM.Common.Interfaces;
using ASCOM.Common;

namespace ASCOM.Simulators
{
	//
	// Your driver's ID is ASCOM.Simulator.Rotator
	//
	// The Guid attribute sets the CLSID for ASCOM.Simulator.Rotator
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rotator from being created and used as the [default] interface
	//
	public class Rotator : IRotatorV3, IAlpacaDevice
	{
		/// <summary>
		/// Driver ID - ClassID and used in the profile
		/// </summary>
		private const string UNIQUE_ID_PROFILE_NAME = "UniqueID";

		//
		// Constructor - Must be public for COM registration!
		//
		public Rotator(int deviceNumber, ILogger logger, IProfile profile)
		{
			try
			{
				RotatorHardware.Initialize(profile);

				DeviceNumber = deviceNumber;

				//This should be replaced by the next bit of code but is semi-unique as a default.
				UniqueID = Name + deviceNumber.ToString();
				//Create a Unique ID if it does not exist
				try
				{
					if (!profile.ContainsKey(UNIQUE_ID_PROFILE_NAME))
					{
						var uniqueid = Guid.NewGuid().ToString();
						profile.WriteValue(UNIQUE_ID_PROFILE_NAME, uniqueid);
					}
					UniqueID = profile.GetValue(UNIQUE_ID_PROFILE_NAME);
				}

				catch (Exception ex)
				{
					logger.LogError($"Rotator {deviceNumber} - {ex.Message}");
				}

				logger.LogInformation($"Rotator {deviceNumber} - UUID of {UniqueID}");

			}

			catch (Exception ex)
			{
				logger.LogError(ex.Message.ToString());
				throw ex;
			}

		}

		public string DeviceName { get => Name; }
		public int DeviceNumber { get; private set; }
		public string UniqueID { get; private set; }

		//
		// PUBLIC COM INTERFACE IRotator IMPLEMENTATION
		//
		#region IDeviceControl Members
		/// <summary>
		/// Invokes the specified device-specific action.
		/// </summary>
		/// <exception cref="MethodNotImplementedException"></exception>
		public string Action(string actionName, string actionParameters)
		{
			throw new ASCOM.ActionNotImplementedException(actionName);
		}

		public void CommandBlind(string command, bool raw)
		{
			throw new ASCOM.MethodNotImplementedException("CommandBlind " + command);
		}

		public bool CommandBool(string command, bool raw)
		{
			throw new ASCOM.MethodNotImplementedException("CommandBool " + command);
		}

		public string CommandString(string command, bool raw)
		{
			throw new ASCOM.MethodNotImplementedException("CommandString " + command);
		}

		public void Dispose()
		{
			//throw new MethodNotImplementedException("Dispose");
		}

		/// <summary>
		/// Gets the supported actions.
		/// </summary>
		public IList<string> SupportedActions
		{
			// no supported actions, return empty array
			get { return new List<string>(); }
		}

		#endregion

		#region IRotator Members
		public bool Connected
		{
			get { return RotatorHardware.Connected; }
			set { RotatorHardware.Connected = value; }
		}

		public string Description
		{
			get { return RotatorHardware.Description; }
		}

		public string DriverInfo
		{
			get { return RotatorHardware.DriverInfo; }
		}

		public string DriverVersion
		{
			get
			{
				Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				return $"{version.Major}.{version.Minor}";
			}
		}

		public short InterfaceVersion
		{
			get { return RotatorHardware.InterfaceVersion; }
		}

		public string Name
		{
			get { return RotatorHardware.RotatorName; }
		}

		public bool CanReverse
		{
			get { return RotatorHardware.CanReverse; }
		}

		public void Halt()
		{
			RotatorHardware.Halt();
		}

		public bool IsMoving
		{
			get { return RotatorHardware.Moving; }
		}

		public void Move(float position)
		{
			RotatorHardware.Move(position);
		}

		public void MoveMechanical(float position)
		{
			RotatorHardware.MoveAbsolute(position);
		}

		public float MechanicalPosition
		{
			get { return RotatorHardware.Position; }
		}

		public bool Reverse
		{
			get { 
				if(InterfaceVersion < 3 && !CanReverse)
                {
					throw new PropertyNotImplementedException();
                }
				return RotatorHardware.Reverse; 
			}
			set {
				if (InterfaceVersion < 3 && !CanReverse)
				{
					throw new PropertyNotImplementedException();
				}
				RotatorHardware.Reverse = value; 
			}
		}

		public float StepSize
		{
			get { return RotatorHardware.StepSize; }
		}

		public float TargetPosition
		{
			get { return RotatorHardware.TargetPosition; }
		}

		public float Offset
		{
			get => RotatorHardware.SyncOffset;
		}

		public float Position
		{
			get
			{
				return (MechanicalPosition + Offset + 360) % 360;
			}
		}

		public void MoveAbsolute(float position)
		{
			MoveMechanical((position - Offset + 36000) % 360);
		}

		public void Sync(float position)
		{
			RotatorHardware.SyncOffset = position - MechanicalPosition;
		}


		#endregion
	}
}
