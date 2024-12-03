import React, { useEffect, useState } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, TextField, Button, Grid, MenuItem } from '@material-ui/core';
import axios from 'axios';

export default function AddScheduleForm({ open, handleClose, movieId }) {
    const [date, setDate] = useState('');
    const [price, setPrice] = useState('');
    const [theatre, setTheatre] = useState('');
    const [timeslot, setTimeslot] = useState('');
    const [theatres, setTheatres] = useState([]);
    const [timeslots, setTimeslots] = useState([]);

    useEffect(() => {
        // Fetch theatres and timeslots for dropdowns
        axios.get('https://localhost:5001/api/theatres').then((response) => {
            setTheatres(response.data);
        });
        axios.get('https://localhost:5001/api/timeslots').then((response) => {
            setTimeslots(response.data);
        });
    }, []);

    const handleSubmit = () => {
        const scheduleData = {
            date,
            price,
            theatreId: theatre,
            timeslotId: timeslot,
            movieId,
        };

        axios
            .post('https://localhost:5001/api/schedules', scheduleData)
            .then((response) => {
                if (response.status === 201) {
                    alert('Schedule added successfully!');
                    handleClose();
                }
            })
            .catch((error) => alert('Failed to add schedule'));
    };

    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle>Add Schedule</DialogTitle>
            <DialogContent>
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <TextField fullWidth label="Date" type="date" InputLabelProps={{ shrink: true }} 
                        value={date} onChange={(e) => setDate(e.target.value)} 
                        inputProps={{
                            min: new Date().toISOString().split("T")[0]
                        }}/>
                    </Grid>
                    <Grid item xs={12}>
                        <TextField fullWidth label="Price" type="number" value={price} onChange={(e) => setPrice(e.target.value)} />
                    </Grid>
                    <Grid item xs={12}>
                        <TextField fullWidth select label="Theatre" value={theatre} onChange={(e) => setTheatre(e.target.value)}>
                            {theatres.map((theatre) => (
                                <MenuItem key={theatre.id} value={theatre.id}>
                                    {theatre.name} screen {theatre.screenNumber} at {theatre.location}
                                </MenuItem>
                            ))}
                        </TextField>
                    </Grid>
                    <Grid item xs={12}>
                        <TextField fullWidth select label="Timeslot" value={timeslot} onChange={(e) => setTimeslot(e.target.value)}>
                            {timeslots.map((timeslot) => (
                                <MenuItem key={timeslot.id} value={timeslot.id}>
                                    From {timeslot.startTime} To {timeslot.endTime}
                                </MenuItem>
                            ))}
                        </TextField>
                    </Grid>
                </Grid>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose} color="secondary">
                    Cancel
                </Button>
                <Button onClick={handleSubmit} color="primary">
                    Add Schedule
                </Button>
            </DialogActions>
        </Dialog>
    );
}
