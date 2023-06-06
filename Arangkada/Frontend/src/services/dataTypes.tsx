export type Operator = {
  id: string,
  fullName: string,
  username: string,
  password: string,
  email: string,
  verificationStatus: boolean,
  verificationCode: string,
  drivers: number,
  vehicles: number,
}

export type PostOperator = {
  fullName: string,
  username: string,
  password: string,
  email: string
}

export type PutOperator = {
  fullName: string,
  username: string,
  password: string,
  email: string
}

export type Vehicle = {
  id: string,
  operatorName: string,
  crNumber: string,
  plateNumber: string,
  bodyType: string,
  make: string,
  distinctionLabel: string,
  rentFee: number,
  rentStatus: boolean
  [key: string]: string | number | boolean;
}

export type PostVehicle = {
  operatorName: string,
  crNumber: string,
  plateNumber: string,
  bodyType: string,
  make: string,
  distinctionLabel: string,
  rentFee: number,
  rentStatus: boolean
}

export type Driver = {
  id: string,
  operatorName: string,
  vehicleAssigned: string,
  fullName: string,
  address: string,
  contactNumber: string,
  licenseNumber: string,
  expirationDate: string,
  dlCodes: string,
  [key: string]: string | number | boolean;
};

export type PostDriver = {
  operatorName: string,
  fullName: string,
  address: string,
  contactNumber: string,
  licenseNumber: string,
  expirationDate: string,
  dlCodes: string
}

export type PutDriver = {
  fullName: string,
  address: string,
  contactNumber: string,
  licenseNumber: string,
  expirationDate: string,
  dlCodes: string
}

export type Transaction = {
  id: string,
  operatorName: string,
  driverName: string,
  amount: number,
  date: string,
  [key: string]: string | number | boolean;
}

export type PostTransaction = {
  operatorName: string,
  driverName: string,
  amount: number,
  date: string
}

export type PutTransaction = {
  amount: number,
  date: string
}