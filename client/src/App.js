import "./App.css";
import { Switch, Route } from "react-router-dom";
import Navbar from "./components/Navbar/Navbar";
import Footer from "./components/Footer/Footer";
import Login from "./pages/Login/Login";
import Signup from "./pages/Signup/Signup";
import Home from "./pages/Home/Home";
import MovieDetails from "./components/MovieDetails/MovieDetails";
import ReservationList from "./components/Admin/ReservationList/ReservationList";
import MovieList from "./components/Admin/MovieList/MovieList";
import TheatreList from "./components/Admin/TheatreList/TheatreList";
import Checkin from "./components/Admin/Checkin";

// const theme = createTheme({
//   palette: {
//     primary: {
//       main: "#1976d2", // Custom primary color
//     },
//     secondary: {
//       main: "#dc004e", // Custom secondary color
//     },
//     background: {
//       default: "#f5f5f5", // Custom background color
//     },
//   },
//   typography: {
//     fontSize: "2.5rem",
//      h1: {
//      fontWeight: "bold",
//     },
//     body1: {
//       fontSize: "1rem",
//     },
//   },
// });

function App() {
  return (
    <div className="App">
      <div className="container">
        <Navbar />
        <Switch>
          <Route exact path="/" component={Home} />
          <Route exact path="/login" component={Login} />
          <Route exact path="/signup" component={Signup} />
          <Route exact path="/movies/:id" component={MovieDetails} />
          <Route exact path="/admin/reservations" component={ReservationList} />
          <Route exact path="/admin/movies" component={MovieList} />
          <Route exact path="/admin/checkin" component={Checkin} />
        </Switch>
      </div>
      <div className="subContainer">
        <Footer />
      </div>
    </div>
  );
}

export default App;
