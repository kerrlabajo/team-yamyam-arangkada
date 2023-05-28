import { Box, Typography } from "@mui/material";
import { useContext, useEffect, useState } from "react";
import Footer from "../../components/shared/Footer";
import PageHeader from "../../components/shared/PageHeader";
import DriverCardList from "../../components/driver/views/DriverCardList";
import SearchFilterForm from "../../components/shared/SearchFilterForm";
import { Driver } from "../../services/dataTypes";
import { useNavigate } from "react-router-dom";
import { driverService } from "../../services/driverService";
import { UserContext, UserContextType } from "../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../contexts/MessageContext";

const DriverListPage = () => {
  const { user } = useContext(UserContext) as UserContextType;
  const navigate = useNavigate();
  const { setMessage } = useContext(MessageContext) as MessageContextType;
  const [filteredDrivers, setFilteredDrivers] = useState<Driver[]>([]);
  const [driverList, setDriverList] = useState<Driver[]>([]);
  const [data, setData] = useState<Partial<Driver>>({
    fullName: "",
    address: "",
    vehicleAssigned: "",
    contactNumber: "",
    licenseNumber: "",
    expirationDate: "",
    dlCodes: ""
  });

  useEffect(() => {
    if (user !== null) {
      driverService
        .getDriversByOperator(user.id)
        .then((response) => {
          setDriverList(response);
        })
        .catch((error) => {
          setMessage(error.response.data || "Failed to retrieve drivers.")
        });
    }
    else{
      console.log('No user logged in')
      setMessage("No user logged in");
      window.localStorage.removeItem("ARANGKADA_USER");
      navigate('/', { replace: true });
    }
  }, [user, setMessage, navigate]);

  useEffect(() => {
    setFilteredDrivers(driverList);
  }, [driverList]);

  const handleFilterSubmit = (filters: Partial<Driver>) => {
    const filtered = driverList.filter((driver) => {
      for (const key in filters) {
        if (
          filters.hasOwnProperty(key) &&
          driver.hasOwnProperty(key) &&
          filters[key] !== undefined &&
          filters[key] !== null &&
          String(driver[key as keyof Driver])
            .toLowerCase()
            .includes(String(filters[key as keyof Driver]).toLowerCase())
        ) {
          return true;
        }
      }
      return false;
    });
  
    setFilteredDrivers(filtered);
  };
  

  const handleFilterClear = () => {
    setFilteredDrivers(driverList);
    setData({
      fullName: "",
      address: "",
      contactNumber: "",
      licenseNumber: "",
      expirationDate: "",
      dlCodes: ""
    });
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setData((prevData) => ({ ...prevData, [name]: value }));
  };

  return (
    <>
      <Box mt="12px" sx={{ minHeight: "80vh" }}>
        <PageHeader title="Drivers" />
        <br />
        <SearchFilterForm
          objectProperties={Object.keys(data) as (keyof Driver)[]}
          data={data}
          handleInputChange={handleInputChange}
          handleFilterSubmit={handleFilterSubmit}
          handleFilterClear={handleFilterClear}
        />
        <br />
        {filteredDrivers.length !== 0 ? (
          <DriverCardList drivers={filteredDrivers} />
        ) : (
          <Typography variant="body1" color="text.secondary">
            No drivers added.
          </Typography>
        )}
      </Box>
      <Footer name="John William Miones" course="BSCS" section="F1" />
    </>
  );
};

export default DriverListPage;
