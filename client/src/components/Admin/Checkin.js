import React, { useState } from 'react';
import { TextField, Button, Typography } from '@material-ui/core';
import { axiosApiInstance as API } from '../../utils/axiosConfig';

const Checkin = () => {
    const [ticketId, setTicketId] = useState('');
    const [status, setStatus] = useState('');
    const [statusColor, setStatusColor] = useState(''); // To store text color based on the result

    const handleCheckin = async () => {
        try {
            const response = await API.get(`/Bookings/${ticketId}`);
            const stat = response.data.status;
            if (stat === 'ACTIVE') {
                setStatus('Check-in successful');
                setStatusColor('green'); // Success color
            } else {
                setStatus(`Failed to check-in because ticket is in ${stat} status`);
                setStatusColor('red'); // Failure color
            }
        } catch (error) {
            setStatus('Failed to check-in');
            setStatusColor('red'); // Failure color
        }
    };

    return (
        <div
            style={{
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center',
                height: '100vh',
                textAlign: 'center',
            }}
        >
            {/* Page Heading */}
            <Typography variant="h4" style={{ marginBottom: '20px' }}>
                Ticket Check-In
            </Typography>

            {/* Input Field */}
            <TextField
                label="Ticket ID"
                variant="outlined"
                value={ticketId}
                onChange={(e) => setTicketId(e.target.value?.trim())}
                style={{ marginBottom: '20px', width: '300px' }}
            />

            {/* Check-In Button */}
            <Button
                variant="contained"
                color="primary"
                onClick={handleCheckin}
                style={{ marginBottom: '20px' }}
            >
                Check In
            </Button>

            {/* Status Message */}
            {status && (
                <Typography
                    variant="h6"
                    style={{ color: statusColor, fontSize: '24px', marginTop: '20px' }}
                >
                    {status}
                </Typography>
            )}
        </div>
    );
};

export default Checkin;
