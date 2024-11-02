import React, { useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";

import { Edit, Delete } from "@material-ui/icons";
import { axiosApiInstance as API } from "../../../utils/axiosConfig";
import axios from "axios";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  IconButton,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
} from "@material-ui/core";

const useStyles = makeStyles({
  table: {
    minWidth: 650,
  },
  addButton: {
    marginBottom: 20,
  },
});

const TheatreList = () => {
  const classes = useStyles();
  const [theatres, setTheatres] = useState([]);
  const [open, setOpen] = useState(false);
  const [currentTheatre, setCurrentTheatre] = useState({
    name: "",
    location: "",
    city: "",
    state: "",
  });

  useEffect(() => {
    fetchTheatres();
  }, []);

  const fetchTheatres = async () => {
    try {
      const response = await API.get("theatres");
      setTheatres(response.data);
    } catch (error) {
      console.error("Error fetching theatres:", error);
    }
  };

  const handleAddClick = () => {
    setCurrentTheatre({ name: "", location: "" });
    setOpen(true);
  };

  const handleEditClick = (theatre) => {
    setCurrentTheatre(theatre);
    setOpen(true);
  };

  const handleDeleteClick = async (id) => {
    try {
      await API.delete(`theatres/${id}`);
      fetchTheatres();
    } catch (error) {
      console.error("Error deleting theatre:", error);
    }
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleSave = async () => {
    try {
      if (currentTheatre.id) {
        await API.put(`theatres/${currentTheatre.id}`, currentTheatre);
      } else {
        await API.post("theatres", currentTheatre);
      }
      fetchTheatres();
      setOpen(false);
    } catch (error) {
      console.error("Error saving theatre:", error);
    }
  };

  return (
    <div>
      <Button
        variant="contained"
        color="primary"
        className={classes.addButton}
        onClick={handleAddClick}
      >
        Add Theatre
      </Button>
      <TableContainer component={Paper}>
        <Table className={classes.table} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell>Theatre Name</TableCell>
              <TableCell>Location</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {theatres.map((theatre) => (
              <TableRow key={theatre.id}>
                <TableCell>{theatre.name}</TableCell>
                <TableCell>{theatre.location}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleEditClick(theatre)}>
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDeleteClick(theatre.id)}>
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">
          {currentTheatre.id ? "Edit Theatre" : "Add Theatre"}
        </DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Theatre Name"
            type="text"
            fullWidth
            value={currentTheatre.name}
            onChange={(e) =>
              setCurrentTheatre({ ...currentTheatre, name: e.target.value })
            }
          />

          <TextField
            margin="dense"
            label="Location"
            type="text"
            fullWidth
            value={currentTheatre.location}
            onChange={(e) =>
              setCurrentTheatre({ ...currentTheatre, location: e.target.value })
            }
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="primary">
            Cancel
          </Button>
          <Button onClick={handleSave} color="primary">
            Save
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default TheatreList;
