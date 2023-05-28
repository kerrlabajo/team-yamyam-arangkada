import { Box, Typography } from "@mui/material";
import { useContext, useEffect, useState } from "react";
import Footer from "../../components/shared/Footer";
import PageHeader from "../../components/shared/PageHeader";
import VehicleCardList from "../../components/vehicle/views/VehicleCardList";
import SearchFilterForm from "../../components/shared/SearchFilterForm";
import { Vehicle } from "../../services/dataTypes";
import { vehicleService } from "../../services/vehicleService";
import { UserContext, UserContextType } from "../../contexts/UserContext";
import { MessageContext, MessageContextType } from "../../contexts/MessageContext";
import { useNavigate } from "react-router-dom";

const VehicleListPage = () => {
  const { user } = useContext(UserContext) as UserContextType;
  const { setMessage } = useContext(MessageContext) as MessageContextType;
  const navigate = useNavigate();
  const [filteredVehicles, setFilteredVehicles] = useState<Vehicle[]>([]);
  const [myVehicles, setVehicles] = useState<Vehicle[]>([]);
  const [data, setData] = useState<Partial<Vehicle>>({
    crNumber: "",
    plateNumber: "",
    bodyType: "",
    make: "",
    rentFee: 0,
    rentStatus: false,
  });

  useEffect(() => {
    if (user !== null) {
      vehicleService
        .getVehiclesByOperator(user.id)
        .then((response) => {
          setVehicles(response);
        })
        .catch((error) => {
          setMessage(error.response.data || "Failed to retrieve vehicles.")
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
    setFilteredVehicles(myVehicles);
  }, [myVehicles]);

  const handleFilterSubmit = (filters: Partial<Vehicle>) => {
    const filtered = myVehicles.filter((vehicle) => {
      for (const key in filters) {
        if (
          filters.hasOwnProperty(key) &&
          vehicle.hasOwnProperty(key) &&
          filters[key] !== undefined &&
          filters[key] !== null &&
          String(vehicle[key]).toLowerCase().includes(
          String(filters[key]).toLowerCase()
          )
        ) {
          return true;
        }
      }
      return false;
    });

    setFilteredVehicles(filtered);
  };

  const handleFilterClear = () => {
    setFilteredVehicles(myVehicles);
    setData({
      crNumber: "",
      plateNumber: "",
      bodyType: "",
      make: "",
      rentFee: 0,
      rentStatus: false,
    });
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setData((prevData) => ({ ...prevData, [name]: value }));
  };

  return (
    <>
      <Box mt="12px" sx={{ minHeight: "80vh" }}>
        <PageHeader title="Vehicles" />
        <br />
        <SearchFilterForm
          objectProperties={Object.keys(data) as (keyof Vehicle)[]}
          data={data}
          handleInputChange={handleInputChange}
          handleFilterSubmit={handleFilterSubmit}
          handleFilterClear={handleFilterClear}
        />
        <br />
        {filteredVehicles.length !== 0 ? (
          <VehicleCardList vehicles={filteredVehicles} />
        ) : (
          <Typography variant="body1" color="text.secondary">
            No vehicles added.
          </Typography>
        )}
      </Box>
      <Footer name="Olivein Kurl Potolin" course="BSCS" section="F1" />
    </>
  );
};

export default VehicleListPage;